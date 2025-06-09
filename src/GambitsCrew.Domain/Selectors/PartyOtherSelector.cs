using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record PartyOtherSelector(
    List<ICondition> PX
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        foreach(var pt in ctx.Party.Skip(1))
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
