using EliteMMO.API;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.CrewMembers;

public record CrewMember(
    string Name,
    List<IGambit> Gambits
) : ICrewMember
{
    public async Task RunAsync(EliteAPI api, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var ctx = new CrewContext(api);
            await RunCycleAsync(ctx, cancellationToken);
        }
    }

    private async Task RunCycleAsync(CrewContext ctx, CancellationToken cancellationToken)
    {
        foreach (var gambit in Gambits)
        {
            if (await gambit.TryRunAsync(ctx, cancellationToken))
            {
                return;
            }
        }

        await Task.Delay(500, cancellationToken);
    }
}
