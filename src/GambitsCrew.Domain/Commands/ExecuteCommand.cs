using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record ExecuteCommand(
    string Execute
) : ICommand
{

    public Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        api.SendString(Execute);
        return Task.FromResult(true);
    }
}
