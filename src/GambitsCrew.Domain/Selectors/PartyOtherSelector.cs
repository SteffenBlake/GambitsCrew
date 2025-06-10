using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record PartyOtherSelector(
    List<ICondition> PX
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        foreach(var pt in api.PartyEntities().Skip(1))
        {
            ctx.ContextualEntity = pt;
            ctx.ContextualTarget = pt.Name;
            if (PX.All(condition => condition.Eval(ctx)))
            {
                return true;
            }
        }

        return false;
    }
}
