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
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = TargetType.Enemy;
        if (!ctx.EnsureContextualTarget(api, null, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for item '{WS.Name}'");
        }

        var ws = api.GetWeaponskill(WS.Name);

        var playerMember = api.PlayerMember();
        if (playerMember == null)
        {
            return GambitFail.Default;
        }
        // Check TP
        if (playerMember.CurrentTP < WS.TP)
        {
            return GambitFail.Default;
        }

        var playerEntity = api.PlayerEntity(playerMember);
        // Check busy
        if (playerEntity.IsBusy())
        {
            return GambitFail.Default;
        }

        // Check Idle/Engaged 
        if (!playerEntity.IsIdle())
        {
            return GambitFail.Default;
        }

        // Check too far
        if (ctx.ContextualEntity.Distance > 5.9f)
        {
            return GambitFail.Default;
        }

        // Check can use WS
        if (!api.PlayerHasWeaponskill(ws.ID))
        {
            return GambitFail.Default;
        }

        api.SendString($"/ws \"{WS.Name}\" {ctx.ContextualTarget}");

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        // Run option added wait time
        if (WS.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(WS.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID, ws.ID);
    }
}


