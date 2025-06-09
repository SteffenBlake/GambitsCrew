using System.Diagnostics;
using System.Text.Json;
using EliteMMO.API;
using GambitsCrew.Domain;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GambitsCrew.CLI.Run;

public static class RunCmd
{
    internal static async Task<int> RunAsync(RunOptions options)
    {
        var cts = new CancellationTokenSource();

        var path = Path.Combine(Directory.GetCurrentDirectory(), options.Path);
        var cwd = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();

        var fileProvider = new ProjectFileProviderService(cwd);

        var services = new ServiceCollection()
            .AddDomainServices()
            .AddSingleton<IFileProviderService>(fileProvider)
            .BuildServiceProvider();

        var serializerOptions = services.GetRequiredService<JsonSerializerOptionsBuilder>()
            .Compile();

        var data = File.OpenRead(path);
        var deployment = (await JsonSerializer.DeserializeAsync<Deployment>(
                data, serializerOptions
            )) ?? throw new InvalidOperationException(
                $"Unable to serialize deployment '{options.Path}'"
            );

        List<Task> runners = [];

        var crew = deployment.Crew.ToDictionary(c => c.Name, c => c);

        var processes = Process.GetProcessesByName("pol");
        foreach (var process in processes)
        {
            var api = new EliteAPI(process.Id);
            if (crew.TryGetValue(api.Player.Name, out var crewMember))
            {
                runners.Add(crewMember.RunAsync(api, cts.Token));
            }
        }

        await Task.WhenAll(runners);

        return 0;
    }
}
