using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record OrSelector(
    List<ISelector> Or
) : ISelector 
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        return Or.Any(cond => cond.Eval(ctx, api));
    }
}
