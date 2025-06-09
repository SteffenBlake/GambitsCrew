using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Commands;

public record AttackCommandInfo(
    int? Wait = null
);

public record AttackCommand(
    AttackCommandInfo Attack
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        if (ctx.ContextualEntity == null)
        {
            throw new InvalidOperationException("Entity didnt get set?");
        }

        // Check if enemy
        var type = (TargetType)ctx.ContextualEntity.Type;
        if (!type.HasFlag(TargetType.Enemy))
        {
            return false;
        }

        // Check already engaged
        var status = (EntityStatus)ctx.Player.Status;
        if (status.HasFlag(EntityStatus.Engaged))
        {
            return false;
        }

        // Check that we are Idle
        if (!status.HasFlag(EntityStatus.Idle))
        {
            return false;
        }

        // TODO : Double check this distance is right
        // Check too far
        if (ctx.ContextualEntity.Distance > 40)
        {
            return false;
        }

        ctx.Api.ThirdParty.SendString("/attack");

        // Wait for engaged status to be true
        do
        {
            await Task.Delay(200, cancellationToken);
        } while (!status.HasFlag(EntityStatus.Engaged));

        // Run option added wait time
        if (Attack.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Attack.Wait.Value), cancellationToken);
        }

        return true;
    }
}
