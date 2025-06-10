using EliteMMO.API;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record AttackCommandInfo(
    int? Wait = null
);

public record AttackCommand(
    AttackCommandInfo Attack
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = TargetType.Enemy;
        if (!ctx.EnsureContextualTarget(api, null, validTargets))
        {
            throw new InvalidOperationException("Could not infer a valid target for Attack");
        }

        // Check if enemy
        var type = (TargetType)ctx.ContextualEntity.Type;
        if (!type.HasFlag(TargetType.Enemy))
        {
            return false;
        }

        // Check already engaged
        var status = (EntityStatus)api.PlayerEntity.Status;
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

        api.SendString("/attack");

        // Wait for engaged status to be true
        do
        {
            await Task.Delay(500, cancellationToken);
            status = (EntityStatus)api.PlayerEntity.Status;
        } while (!status.HasFlag(EntityStatus.Engaged));

        // Run option added wait time
        if (Attack.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Attack.Wait.Value), cancellationToken);
        }

        return true;
    }
}
