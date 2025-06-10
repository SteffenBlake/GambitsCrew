using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain.Gambits;

public record Gambit(
    ISelector When,
    ICommand Do
) : IGambit
{
    public async Task<bool> TryRunAsync(
        IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var ctx = new GambitContext();
        if (When.Eval(ctx, api))
        {
            return await Do.TryInvokeAsync(ctx, api, cancellationToken);
        }

        return false;
    }
}
