using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record NameCondition(
    List<IStringOperator> Name
) : ICondition
{
    public bool Eval(CrewContext ctx)
    {
        return Name.All(condition => condition.Eval(ctx.ContextualEntity!.Name));
    }
}
