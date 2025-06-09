using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record PetCommandInfo(
    string Name,
    string? Target = null,
    int? Wait = null
);

public record PetCommand(
    PetCommandInfo Pet
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        if (ctx.Pet == null)
        {
            return false;
        }

        if (ctx.PlayerEntity.IsBusy())
        {
            return false;
        }

        var ability = ctx.Api.Resources.GetAbility(Pet.Name, 0) ??
            throw new ArgumentException($"No known Pet Cmd: '{Pet.Name}'");

        var type = (AbilityType)ability.Type;
        if (!type.HasFlag(AbilityType.Pet)
        )
        {
            throw new ArgumentException($"'{Pet.Name}' is not a Pet Cmd"); 
        }

        // Check valid target
        var validTargets = (TargetType)ability.ValidTargets;
        if (!ctx.EnsureContextualTarget(Pet.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Job Ability '{Pet.Name}'");
        }

        if ((validTargets & ctx.ContextualTargetType) <= 0)
        {
            throw new InvalidOperationException(
                $"Tried to use Job Ability '{Pet.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
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

        // Check can use
        if (!ctx.Player.HasAbility(ability.ID))
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity!.Distance > ability.Range)
        {
            return false;
        }

        // Check if on cooldown
        var index = ctx.AbilityIdsIndexed[ability.TimerID];
        if (ctx.Api.Recast.GetAbilityRecast(index) > 0)
        {
            return false;
        }
        
        ctx.Api.ThirdParty.SendString($"/pet {Pet.Name} {ctx.ContextualTarget}");

        // Run option added wait time
        if (Pet.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Pet.Wait.Value), cancellationToken);
        }

        return true;
    }
}
