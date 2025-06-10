using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record AllianceOtherSelector(
    List<ICondition> AX
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        foreach(var a in api.AllianceEntities().Skip(1))
        {
            ctx.ContextualEntity = a;
            ctx.ContextualTarget = a.Name;
            if (AX.All(condition => condition.Eval(ctx)))
            {
                return true;
            }
        }

        return false;
    }
}

