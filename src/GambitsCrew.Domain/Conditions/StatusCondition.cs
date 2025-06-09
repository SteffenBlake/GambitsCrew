using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record StatusCondition(
    List<INumberOperator> Status
) : ICondition
{
    public bool Eval(CrewContext ctx)
    {
        return Status.All(condition => condition.Eval((int)ctx.ContextualEntity!.Status));
    }
}

