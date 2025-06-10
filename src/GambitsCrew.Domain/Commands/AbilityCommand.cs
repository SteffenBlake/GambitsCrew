using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record AbilityCommandInfo(
    string Name,
    string? Target = null,
    int? Wait = null
);

public record AbilityCommand(
    AbilityCommandInfo JA
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var ability = api.GetJobAbility(JA.Name);

        // Check valid target
        var validTargets = (TargetType)ability.ValidTargets;
        if (!ctx.EnsureContextualTarget(api, JA.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Job Ability '{JA.Name}'");
        }

        if (!api.TargetsOverlap(ctx.ContextualEntity, validTargets))
        {
            throw new InvalidOperationException(
                $"Tried to use Job Ability '{JA.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        var playerEntity = api.PlayerEntity;
        // Check busy
        if (playerEntity.IsBusy())
        {
            return false;
        }

        // Check Idle/Engaged 
        var status = (EntityStatus)playerEntity.Status;
        if (!status.HasFlag(EntityStatus.Idle) && !status.HasFlag(EntityStatus.Engaged))
        {
            return false;
        }

        var player = api.PlayerMember;
        // Check TP
        if (player.CurrentTP < ability.TP)
        {
            return false;
        }

        // Check MP
        if (player.CurrentMP < ability.MP)
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity!.Distance > ability.Range)
        {
            return false;
        }

        // Check can use
        if (!api.PlayerHasAbility(ability.ID))
        {
            return false;
        }

        // Check if on cooldown
        if (api.GetRecast(ability) > 0)
        {
            return false;
        }

        api.SendString($"/ja \"{JA.Name}\" {ctx.ContextualTarget}");

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        // Run option added wait time
        if (JA.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(JA.Wait.Value), cancellationToken);
        }

        return true;
    }
}
