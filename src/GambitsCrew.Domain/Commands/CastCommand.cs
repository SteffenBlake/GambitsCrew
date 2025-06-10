using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record CastCommandInfo(
    string Name,
    string? Target = null,
    int? Wait = null
);

public record CastCommand(
    CastCommandInfo Cast
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var player = api.PlayerEntity;
        // Check if in the middle of another action
        if (player.IsBusy())
        {
            return false;
        }

        // Check Idle/Engaged 
        var status = (EntityStatus)player.Status;
        if (status != EntityStatus.Idle)
        {
            return false;
        }

        var spell = api.GetSpell(Cast.Name);

        var playerMember = api.PlayerMember;
        // Check have enough mp
        if (playerMember.CurrentMP < spell.MPCost)
        {
            return false;
        }

        // Check if we can cast this spell at all
        if (!api.PlayerHasSpell(spell.ID))
        {
            return false;
        }

        // Check if on cooldown
        if (api.GetSpellRecast(spell.Index) > 0)
        {
            return false;
        }

        // Check distance
        if (ctx.ContextualEntity!.Distance >= spell.Range)
        {
            return false;
        }

        // Check valid target
        var validTargets = (TargetType)spell.ValidTargets;
        if (!ctx.EnsureContextualTarget(api, Cast.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Spell '{Cast.Name}'");
        }

        api.SendString($"/ma \"{Cast.Name}\" {ctx.ContextualTarget}");

        await Task.Delay(300, cancellationToken);

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        if (Cast.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Cast.Wait.Value), cancellationToken);
        }

        return true;
    }
}

