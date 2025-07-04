using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record HppCondition(
    List<INumberOperator> HPP
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        return HPP.All(condition => condition.Eval(ctx.ContextualEntity.HealthPercent));
    }
}
