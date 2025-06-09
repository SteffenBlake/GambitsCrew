using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record HppCondition(
    List<INumberOperator> HPP
) : ICondition
{
    public bool Eval(CrewContext ctx)
    {
        return HPP.All(condition => condition.Eval(ctx.ContextualEntity!.HealthPercent));
    }
}
