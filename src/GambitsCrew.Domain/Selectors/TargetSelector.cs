using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record TargetSelector(
    List<ICondition> T 
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        var target = api.TargetEntity();
        if (target == null)
        {
            return false;
        }

        ctx.ContextualEntity = target;
        ctx.ContextualTarget = "<t>";

        return T.All(condition => condition.Eval(ctx));
    }
}
