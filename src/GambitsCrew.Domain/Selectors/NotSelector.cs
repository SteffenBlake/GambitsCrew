using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record NotSelector(
    ISelector Not
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        return !Not.Eval(ctx);
    }
}

