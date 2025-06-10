using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record AndSelector(
    List<ISelector> And
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        return And.All(cond => cond.Eval(ctx, api));
    }
}
