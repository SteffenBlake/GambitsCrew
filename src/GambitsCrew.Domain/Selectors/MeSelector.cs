using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record MeSelector(
    List<ICondition> Me
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        ctx.ContextualEntity = api.PlayerEntity;
        ctx.ContextualTarget = "<me>";

        return Me.All(condition => condition.Eval(ctx));
    }
}
