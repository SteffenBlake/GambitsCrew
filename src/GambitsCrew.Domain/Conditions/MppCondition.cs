using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record MppCondition(
    List<INumberOperator> MPP
) : ICondition
{
    public bool Eval(GambitContext ctx)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        return MPP.All(condition => condition.Eval(ctx.ContextualEntity.HealthPercent));
    }
}
