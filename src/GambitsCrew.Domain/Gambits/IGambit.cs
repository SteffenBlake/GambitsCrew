using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain.Gambits;

public interface IGambit
{
    ISelector When { get; init; }

    ICommand Do { get; init; }

    Task<IGambitResult> TryRunAsync(
        IEliteAPI api, CancellationToken cancellationToken
    );
}

