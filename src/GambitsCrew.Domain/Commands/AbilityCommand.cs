using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
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
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        var ability = ctx.Api.Resources.GetAbility(JA.Name, 0) ??
            throw new ArgumentException($"No known Ability: '{JA.Name}'");

        var type = (AbilityType)ability.Type;
        if (
            type.HasFlag(AbilityType.Weaponskill) ||
            type.HasFlag(AbilityType.Pet)
        )
        {
            throw new ArgumentException($"'{JA.Name}' is not a Job Ability"); 
        }

        // Check valid target
        var validTargets = (TargetType)ability.ValidTargets;
        if (!ctx.EnsureContextualTarget(JA.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Job Ability '{JA.Name}'");
        }

        if ((validTargets & ctx.ContextualTargetType) <= 0)
        {
            throw new InvalidOperationException(
                $"Tried to use Job Ability '{JA.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        // Check busy
        if (ctx.PlayerEntity.IsBusy())
        {
            return false;
        }

        // Check TP
        if (ctx.Player.TP < ability.TP)
        {
            return false;
        }

        // Check MP
        if (ctx.Player.MP < ability.MP)
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity!.Distance > ability.Range)
        {
            return false;
        }

        // Check can use
        if (!ctx.Player.HasAbility(ability.ID))
        {
            return false;
        }

        // Check if on cooldown
        var index = ctx.AbilityIdsIndexed[ability.TimerID];
        if (ctx.Api.Recast.GetAbilityRecast(index) > 0)
        {
            return false;
        }

        ctx.Api.ThirdParty.SendString($"/ja {JA.Name} {ctx.ContextualTarget}");

        // Wait for busy to finish
        do
        {
            await Task.Delay(100, cancellationToken);
        } while (ctx.PlayerEntity.IsBusy());

        // Run option added wait time
        if (JA.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(JA.Wait.Value), cancellationToken);
        }

        return true;
    }
}
