using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record AllianceSelector(
    List<ICondition> A
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        foreach(var a in api.AllianceEntities())
        {
            ctx.ContextualEntity = a;
            ctx.ContextualTarget = a.Name;
            if (A.All(condition => condition.Eval(ctx)))
            {
                return true;
            }
        }

        return false;
    }
}

