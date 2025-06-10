using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Selectors;

public record ScanSelector(
    List<ICondition> Scan
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        foreach(var entity in api.ScanEnemies())
        {
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
