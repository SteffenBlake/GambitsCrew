using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public interface ISelector
{
    bool Eval(GambitContext ctx, IEliteAPI api);
}
