using System.Text.Json;
using GambitsCrew.Domain;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GambitsCrew.CLI.Run;

public static class RunCmd
{
    internal static async Task<int> RunAsync(RunOptions options)
    {
        var path = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), options.Path)
        );
        var cwd = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();
        options.Path = path;

        Console.WriteLine(
            $"Using CWD: '{cwd}'"
        );

        var fileProvider = new ProjectFileProviderService(cwd);

        var app = Host.CreateApplicationBuilder();

        app.Services
            .AddDomainServices()
            .AddSingleton<IFileProviderService>(fileProvider)
            .AddSingleton(options)
            .BuildServiceProvider();

        await app.Build().RunAsync();

        return 0;
    }
}
