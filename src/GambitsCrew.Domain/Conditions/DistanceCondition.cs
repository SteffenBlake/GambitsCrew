using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record DistanceCondition(
    List<INumberOperator> Distance
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        var value = (int)Math.Floor(ctx.ContextualEntity.Distance);
        return Distance.All(condition => condition.Eval(value));
    }
}
