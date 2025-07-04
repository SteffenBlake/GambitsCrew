using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public interface ICommand 
{
    Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    );
}
