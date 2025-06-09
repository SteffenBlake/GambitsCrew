using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record MppCondition(
    List<INumberOperator> MPP
) : ICondition
{
    public bool Eval(CrewContext ctx)
    {
        return MPP.All(condition => condition.Eval(ctx.ContextualEntity!.HealthPercent));
    }
}
