using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record DistanceCondition(
    List<INumberOperator> Distance
) : ICondition
{
    public bool Eval(CrewContext ctx)
    {
        var value = (int)Math.Floor(ctx.ContextualEntity!.Distance);
        return Distance.All(condition => condition.Eval(value));
    }
}
