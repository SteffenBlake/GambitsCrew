using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record OrSelector(
    List<ISelector> Or
) : ISelector 
{
    public bool Eval(CrewContext ctx)
    {
        return Or.Any(cond => cond.Eval(ctx));
    }
}
