using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record TpCondition(
    List<INumberOperator> TP
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        return TP.All(condition => condition.Eval(ctx.ContextualEntity.HealthPercent));
    }
}

