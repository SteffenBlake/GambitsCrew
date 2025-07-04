using EliteMMO.API;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record AttackCommandInfo(
    int? Wait = null
);

public record AttackCommand(
    AttackCommandInfo Attack
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = TargetType.Enemy;
        if (!ctx.EnsureContextualTarget(api, null, validTargets))
        {
            throw new InvalidOperationException("Could not infer a valid target for Attack");
        }

        // Check if enemy
        var type = (EntityTypes)ctx.ContextualEntity.Type;
        if (type != EntityTypes.Npc2)
        {
            return GambitFail.Default;
        }

        var playerEntity = api.PlayerEntity();
        if (playerEntity == null)
        {
            return GambitFail.Default;
        }

        // Check already engaged
        var status = (EntityStatus?)playerEntity.Status;
        if (status.Value.HasFlag(EntityStatus.Engaged))
        {
            return GambitFail.Default;
        }

        // Check that we are Idle
        if (!playerEntity.IsIdle())
        {
            return GambitFail.Default;
        }

        // Check too far
        if (ctx.ContextualEntity.Distance >= 29)
        {
            return GambitFail.Default;
        }

        api.SendString("/attack");

        var timeoutTask = Task.Delay(3000, cancellationToken);

        // Wait for engaged status to be true
        do
        {
            var pauseTask = Task.Delay(500, cancellationToken);
            var next = await Task.WhenAny(timeoutTask, pauseTask);
            if (next == timeoutTask)
            {
                break;
            }
            await Task.Delay(500, cancellationToken);
            status = (EntityStatus?)api.PlayerEntity()?.Status;

        } while (status != null && !status.Value.HasFlag(EntityStatus.Engaged));

        status = (EntityStatus?)api.PlayerEntity()?.Status;
        if (status == null || !status.Value.HasFlag(EntityStatus.Engaged))
        {
            return GambitFail.Default;
        }

        // Run option added wait time
        if (Attack.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Attack.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID);
    }
}