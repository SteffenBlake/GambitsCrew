using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record NameCondition(
    List<IStringOperator> Name
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }
        return Name.All(condition => condition.Eval(ctx.ContextualEntity.Name));
    }
}
