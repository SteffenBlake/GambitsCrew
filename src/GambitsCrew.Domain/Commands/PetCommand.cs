using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
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
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        if (api.PetEntity == null)
        {
            return false;
        }

        var playerEntity = api.PlayerEntity;
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

        var ability = api.GetPetSkill(Pet.Name);

        // Check valid target
        var validTargets = (TargetType)ability.ValidTargets;
        if (!ctx.EnsureContextualTarget(api, Pet.Target, validTargets))
        {
            throw new InvalidOperationException(
                $"No target inferred for Job Ability '{Pet.Name}'"
            );
        }

        if (!api.TargetsOverlap(ctx.ContextualEntity, validTargets))
        {
            throw new InvalidOperationException(
                $"Tried to use Pet Skill '{Pet.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        var playerMember = api.PlayerMember;
        // Check TP
        if (playerMember.CurrentTP < ability.TP)
        {
            return false;
        }

        // Check MP
        if (playerMember.CurrentMP < ability.MP)
        {
            return false;
        }

        // Check can use
        if (!api.PlayerHasAbility(ability.ID))
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity!.Distance > ability.Range)
        {
            return false;
        }

        // Check if on cooldown
        if (api.GetRecast(ability) > 0)
        {
            return false;
        }
        
        api.SendString($"/pet \"{Pet.Name}\" {ctx.ContextualTarget}");

        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);

        // Run option added wait time
        if (Pet.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Pet.Wait.Value), cancellationToken);
        }

        return true;
    }
}
