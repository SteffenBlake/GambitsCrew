using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record PartySelector(
    List<ICondition> P 
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        foreach(var pt in ctx.Party)
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
