using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public interface ICommand 
{
    Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    );
}
