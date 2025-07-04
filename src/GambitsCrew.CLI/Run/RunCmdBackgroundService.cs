using System.Text.Json;
using GambitsCrew.Domain;
using GambitsCrew.Domain.Deployments;
using Microsoft.Extensions.Hosting;

namespace GambitsCrew.CLI.Run;

public class RunCmdBackgroundService(
    RunOptions options,
    JsonSerializerOptionsBuilder serializerOptionsBuilder
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var serializerOptions = serializerOptionsBuilder.Compile();

        var data = File.OpenRead(options.Path);
        var deployment = (await JsonSerializer.DeserializeAsync<Deployment>(
                data, serializerOptions, cancellationToken
            )) ?? throw new InvalidOperationException(
                $"Unable to serialize deployment '{options.Path}'"
            );

        await deployment.RunAsync(cancellationToken);
    } 
}
