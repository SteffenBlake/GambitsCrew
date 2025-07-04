using System.Diagnostics;
using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Deployments;

public record Deployment(
    List<ICrewMember> Crew,
    int MaxAttempts = 5,
    int CycleDelayMs = 1000
)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var cts = new CancellationTokenSource();
        var combined = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, cts.Token
        ).Token;

        List<Task> runners = [];

        var crew = Crew.ToDictionary(c => c.Name, c => c);

        var processes = Process.GetProcessesByName("pol");
        foreach (var process in processes)
        {
            var api = new EliteApiWrapper(new EliteAPI(process.Id));
            var playerName = api.PlayerMember.Name;
            if (crew.TryGetValue(playerName, out var crewMember))
            {
                runners.Add(
                    crewMember.RunAsync(
                        api, MaxAttempts, CycleDelayMs, combined
                    )
                );
                crew.Remove(playerName);
            }
        }

        if (crew.Count > 0)
        {
            var failures = string.Join(", ", crew.Keys);
            Console.WriteLine(
                $"Couldn't bind to instance for crew members: {failures}"
            );
        }

        await Extensions.TaskExtensions.WhenAllWithCancellation(
            runners, cts
        );
    }
}
