using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.CrewMembers;

public record CrewMember(
    string Name,
    List<IGambit> Gambits
) : ICrewMember
{
    public async Task RunAsync(IEliteAPI api, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Binding to character: {api.PlayerEntity.Name}"
        );

        while (!cancellationToken.IsCancellationRequested)
        {
            await RunCycleAsync(api, cancellationToken);
        }
    }

    private async Task RunCycleAsync(
        IEliteAPI api, CancellationToken cancellationToken
    )
    {
        foreach (var gambit in Gambits)
        {
            if (await gambit.TryRunAsync(api, cancellationToken))
            {
                return;
            }
        }

        await Task.Delay(2000, cancellationToken);
    }
}
