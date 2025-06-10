using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record PartySelector(
    List<ICondition> P 
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        foreach(var pt in api.PartyEntities())
        {
            ctx.ContextualEntity = pt;
            ctx.ContextualTarget = pt.Name;
            if (P.All(condition => condition.Eval(ctx)))
            {
                return true;
            }
        }

        return false;
    }
}
