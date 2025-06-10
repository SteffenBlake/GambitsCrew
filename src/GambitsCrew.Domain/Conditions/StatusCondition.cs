using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record StatusCondition(
    List<INumberOperator> Status
) : ICondition
{
    public bool Eval(GambitContext ctx)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        return Status.All(condition => condition.Eval((int)ctx.ContextualEntity.Status));
    }
}

