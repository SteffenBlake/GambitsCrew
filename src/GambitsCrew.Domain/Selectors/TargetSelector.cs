using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record TargetSelector(
    List<ICondition> T 
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        if (ctx.Target == null)
        {
            return false;
        }

        ctx.ContextualEntity = ctx.Target;
        ctx.ContextualTarget = "<t>";

        return T.All(condition => condition.Eval(ctx));
    }
}
