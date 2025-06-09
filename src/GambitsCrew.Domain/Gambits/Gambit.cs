using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain.Gambits;

public record Gambit(
    ISelector When,
    ICommand Do
)
{
    public async Task<bool> TryRunAsync(CrewContext ctx, CancellationToken cancellationToken)
    {
        if (When.Eval(ctx))
        {
            return await Do.TryInvokeAsync(ctx, cancellationToken);
        }

        return false;
    }
}
