using System.Diagnostics.CodeAnalysis;
using EliteMMO.API;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Gambits;

public class GambitContext
{
    public EliteAPI.EntityEntry? ContextualEntity { get; set; } = null;
    public string? ContextualTarget { get; set; } = null;
   
    [MemberNotNullWhen(true, nameof(ContextualEntity), nameof(ContextualTarget))]
    public bool EnsureContextualTarget(
        IEliteAPI api, string? targetString, TargetType? validTargets
    )
    {
        if (!string.IsNullOrEmpty(targetString))
        {
            if (TryInferTargetFromString(api, targetString))
            {
                return true;
            }
        }
    
        if (ContextualEntity != null)
        {
            ContextualTarget ??= ContextualEntity.Name;
            if (!validTargets.HasValue)
            {
                return true;
            }

            if (api.TargetsOverlap(ContextualEntity, validTargets.Value))
            {
                return true;
            }
            // If we hit here, it means we had a guessed target but its not actually valid
            // In which case, we will continue trying to best guess the target
            // based on other context
        }
    
        if (validTargets.HasValue)
        {
            if (TryInferTargetFromType(api, validTargets.Value))
            {
                return true;
            }
        }
    
        return false;
    }
    
    [MemberNotNullWhen(true, nameof(ContextualEntity), nameof(ContextualTarget))]
    private bool TryInferTargetFromString(IEliteAPI api, string target)
    {
        var entity = target switch 
        {
            "<me>" => api.PlayerEntity(),
            "<t>" => api.TargetEntity(),
            "<ldr>" => api.PartyLeaderEntity() ?? api.PlayerEntity(),
            "<bt>" => api.BattleTarget(),
            _ => api.AllianceEntities().SingleOrDefault(a => a.Name == target)
        };
    
        if (entity == null)
        {
            return false;
        }
    
        ContextualEntity = entity;
        ContextualTarget = target;
        return true;
    }
    
    [MemberNotNullWhen(true, nameof(ContextualEntity), nameof(ContextualTarget))]
    private bool TryInferTargetFromType(IEliteAPI api, TargetType validTargets)
    {
        if (validTargets == TargetType.Self)
        {
            ContextualEntity = api.PlayerEntity();
            ContextualTarget = "<me>";
            return ContextualEntity != null;
        }
        var target = api.TargetEntity();
        if (target == null)
        {
            return false;
        }
   
        if (api.TargetsOverlap(target, validTargets))
        {
            ContextualEntity = target;
            ContextualTarget = "<t>";
            return true;
        }
    
        return false;
    }
}
