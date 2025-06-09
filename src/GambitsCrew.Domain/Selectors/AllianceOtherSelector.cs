using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record AllianceOtherSelector(
    List<ICondition> AX
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        foreach(var a in ctx.Alliance.Skip(1))
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

