using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record AllCommand(
    List<ICommand> All
) : ICommand
{
    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var success = true;
        var hash = new HashCode();
        // We can't use LINQ's .Any here, because it will short circuit out
        // before every command runs, 
        // and we actually want every command to run though
        foreach(var cmd in All)
        {
            var result = await cmd.TryInvokeAsync(ctx, api, cancellationToken);
            if (result is GambitSuccess s)
            {
                hash.Add(s.Key);
                continue;
            }
            success = false;
        }

        return success ? GambitFail.Default : new GambitSuccess(hash.ToHashCode());
    }
}

