using GambitsCrew.LocalTesting.Deployments;
using Microsoft.Extensions.Hosting;

namespace GambitsCrew.LocalTesting;

public class Runner(
    IHostApplicationLifetime applicationLifetime,
    IDeploymentBuilder deploymentBuilder
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var deployment = deploymentBuilder.Build();

        await deployment.RunAsync(cancellationToken);

        applicationLifetime.StopApplication();
    }
}
