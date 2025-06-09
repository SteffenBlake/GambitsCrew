using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record AndSelector(
    List<ISelector> And
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        return And.All(cond => cond.Eval(ctx));
    }
}
