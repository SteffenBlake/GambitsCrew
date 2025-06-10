using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record NotSelector(
    ISelector Not
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        return !Not.Eval(ctx, api);
    }
}

