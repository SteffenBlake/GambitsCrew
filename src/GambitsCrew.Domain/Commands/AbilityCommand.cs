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
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
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

        var playerMember = api.PlayerMember();
        if (playerMember == null)
        {
            return GambitFail.Default;
        }

        // Check TP
        if (playerMember.CurrentTP < ability.TP)
        {
            return GambitFail.Default;
        }

        // Check MP
        if (playerMember.CurrentMP < ability.MP)
        {
            return GambitFail.Default;
        }

        // Check too far
        if (ctx.ContextualEntity!.Distance > ability.Range)
        {
            return GambitFail.Default;
        }

        // Check can use
        if (!api.PlayerHasAbility(ability.ID))
        {
            return GambitFail.Default;
        }

        // Check if on cooldown
        if (api.GetRecast(ability) > 0)
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

        api.SendString($"/ja \"{JA.Name}\" {ctx.ContextualTarget}");

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        // Run option added wait time
        if (JA.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(JA.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID, ability.ID);
    }
}
