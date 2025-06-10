using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record AnyCommand(
    List<ICommand> Any
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var success = false;
        // We can't use LINQ's .Any here, because it will short circuit out
        // before every command runs, 
        // and we actually want every command to run though
        foreach(var cmd in Any)
        {
            success |= await cmd.TryInvokeAsync(ctx, api, cancellationToken);
        }

        return success;
    }
}
