using System.Text.Json.Serialization;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.CrewMembers;

public record CrewMember(
    string Name,
    List<IGambit> Gambits
) : ICrewMember
{
    [JsonIgnore]
    private int _trackedCommandKey = 0;

    [JsonIgnore]
    private int _trackedAttempts = 0;

    public async Task RunAsync(
        IEliteAPI api, int maxAttempts, int cycleDelayMs, CancellationToken cancellationToken
    )
    {
        Console.WriteLine(
            $"Binding to character: {api.PlayerEntity()!.Name}"
        );

        while (!cancellationToken.IsCancellationRequested)
        {
            await RunCycleAsync(api, maxAttempts, cancellationToken);
            await Task.Delay(cycleDelayMs, cancellationToken);
        }
    }

    private async Task RunCycleAsync(
        IEliteAPI api, int maxAttempts, CancellationToken cancellationToken
    )
    {
        foreach (var gambit in Gambits)
        {
            if (await gambit.TryRunAsync(api, cancellationToken) is GambitSuccess success)
            {
                if (CheckExceededAttempts(maxAttempts, success.Key))
                {
                    DumpAndThrowState(api, gambit);
                }

                return;
            }
        }

        // Nothing hit, so we can reset
        _trackedAttempts = 0;
        _trackedCommandKey = 0;
    }

    private bool CheckExceededAttempts(int maxAttempts, int key)
    {
        if (_trackedCommandKey == key)
        {
            _trackedAttempts++;
        } 
        else 
        {
            _trackedAttempts = 1;
        }
        _trackedCommandKey = key;

        return _trackedAttempts <= maxAttempts;
    }

    private void DumpAndThrowState(IEliteAPI api, IGambit gambit)
    {

    }
}
