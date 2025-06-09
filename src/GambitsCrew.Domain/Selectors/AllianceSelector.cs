using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record AllianceSelector(
    List<ICondition> A
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        foreach(var a in ctx.Alliance)
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

