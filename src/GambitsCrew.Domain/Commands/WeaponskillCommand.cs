using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record WeaponskillCommandInfo(
    string Name,
    int TP = 1000,
    int? Wait = null
);

public record WeaponskillCommand(
    WeaponskillCommandInfo WS
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = TargetType.Enemy;
        if (!ctx.EnsureContextualTarget(api, null, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for item '{WS.Name}'");
        }

        var ws = api.GetWeaponskill(WS.Name);

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

        var playerMember = api.PlayerMember;
        // Check TP
        if (playerMember.CurrentTP < WS.TP)
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity.Distance > 5.9f)
        {
            return false;
        }

        // Check can use WS
        if (!api.PlayerHasWeaponskill(ws.ID))
        {
            return false;
        }

        api.SendString($"/ws \"{WS.Name}\" {ctx.ContextualTarget}");

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        // Run option added wait time
        if (WS.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(WS.Wait.Value), cancellationToken);
        }

        return true;
    }
}


