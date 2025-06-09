using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
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
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        var spell = ctx.Api.Resources.GetSpell(Cast.Name, 0) ??
            throw new ArgumentException($"No known spell: '{Cast.Name}'");

        // Check if in the middle of another action
        if (ctx.PlayerEntity.IsBusy())
        {
            return false;
        }

        // Check Idle/Engaged 
        var status = (EntityStatus)ctx.Player.Status;
        if (!status.HasFlag(EntityStatus.Idle) && !status.HasFlag(EntityStatus.Engaged))
        {
            return false;
        }

        // Check have enough mp
        if (ctx.Player.MP < spell.MPCost)
        {
            return false;
        }

        // Check if we can cast this spell at all
        if (!ctx.Player.HasSpell(spell.ID))
        {
            return false;
        }

        // Check if on cooldown
        if (ctx.Api.Recast.GetSpellRecast(spell.Index) > 0)
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
        if (!ctx.EnsureContextualTarget(Cast.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Spell '{Cast.Name}'");
        }

        if ((validTargets & ctx.ContextualTargetType) <= 0)
        {
            throw new InvalidOperationException(
                $"Tried to cast spell '{spell.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        ctx.Api.ThirdParty.SendString($"/ma {Cast.Name} {ctx.ContextualTarget}");

        await Task.Delay(300, cancellationToken);

        do
        {
            await Task.Delay(100, cancellationToken);
        } while (ctx.PlayerEntity.IsBusy());

        if (Cast.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Cast.Wait.Value), cancellationToken);
        }

        return true;
    }
}

