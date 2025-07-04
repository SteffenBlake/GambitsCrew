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
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var spell = api.GetSpell(Cast.Name);
        var playerMember = api.PlayerMember();
        if (playerMember == null)
        {
            return GambitFail.Default;
        }

        // Check have enough mp
        if (playerMember.CurrentMP < spell.MPCost)
        {
            return GambitFail.Default;
        }

        // Check if we can cast this spell at all
        if (!api.PlayerHasSpell(spell.ID))
        {
            return GambitFail.Default;
        }

        // Check if on cooldown
        if (api.GetSpellRecast(spell.Index) > 0)
        {
            return GambitFail.Default;
        }

        // Check distance
        if (ctx.ContextualEntity!.Distance >= spell.Range)
        {
            return GambitFail.Default;
        }

        // Check valid target
        var validTargets = (TargetType)spell.ValidTargets;
        if (!ctx.EnsureContextualTarget(api, Cast.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Spell '{Cast.Name}'");
        }

        var playerEntity = api.PlayerEntity(playerMember);

        // Check if in the middle of another action
        if (playerEntity.IsBusy())
        {
            return GambitFail.Default;
        }

        // Check Idle/Engaged 
        if (!playerEntity.IsIdle())
        {
            return GambitFail.Default;
        }

        api.SendString($"/ma \"{Cast.Name}\" {ctx.ContextualTarget}");

        await Task.Delay(300, cancellationToken);

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        if (Cast.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Cast.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID, spell.ID);
    }
}

