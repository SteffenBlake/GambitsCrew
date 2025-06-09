using EliteMMO.API;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Selectors;

public record ScanSelector(
    List<ICondition> Scan
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        for (var index = 0; index <= 1024; index++)
        {
            var entity = ctx.Api.Entity.GetEntity(index);
            var entityStatus = (EntityStatus)entity.Status;

            if (entity.Exists() || entityStatus.HasFlag(EntityStatus.Dead))
            {
                continue;
            }

            if (entity.Distance > 49)
            {
                continue;
            }

            ctx.ContextualEntity = entity;
            ctx.ContextualTarget = entity.TargetingIndex.ToString();

            if(Scan.All(condition => condition.Eval(ctx)))
            {
                return true;
            }
        }

        return false;
    }
}
