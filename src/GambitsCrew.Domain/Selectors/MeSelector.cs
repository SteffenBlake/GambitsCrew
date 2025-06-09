using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record MeSelector(
    List<ICondition> Me
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        ctx.ContextualEntity = ctx.PlayerEntity;
        ctx.ContextualTarget = "<me>";

        return Me.All(condition => condition.Eval(ctx));
    }
}
