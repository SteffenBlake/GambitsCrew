using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record StatusCondition(
    List<IStatusOperator> Status
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        var status = (EntityStatus)ctx.ContextualEntity.Status;
        return Status.All(condition => condition.Eval(status));
    }
}

