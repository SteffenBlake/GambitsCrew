using System.Diagnostics;
using System.Text.Json;
using EliteMMO.API;
using GambitsCrew.LocalTesting;
using GambitsCrew.LocalTesting.Deployments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

var config = builder.Configuration.Get<LocalTestingConfig>() ?? throw new JsonException();
builder.Services.AddSingleton(config);

_ = config.DeploymentChoice switch 
{
    DeploymentChoice.SoloPld => builder.Services.AddSingleton<IDeploymentBuilder, SoloPld>(),
    DeploymentChoice.AssistLeader => builder.Services.AddSingleton<IDeploymentBuilder, AssistLeader>(),
    DeploymentChoice.FaceTarget => builder.Services.AddSingleton<IDeploymentBuilder, FaceTarget>(),
    DeploymentChoice.FollowLeader => builder.Services.AddSingleton<IDeploymentBuilder, FollowLeader>(),
    DeploymentChoice.KillFrogs => builder.Services.AddSingleton<IDeploymentBuilder, KillFrogs>(),
    DeploymentChoice.ReapplySpectralJig => builder.Services.AddSingleton<IDeploymentBuilder, ReapplySpectralJig>(),

    _ => throw new IndexOutOfRangeException(nameof(config.DeploymentChoice))
};

builder.Services.AddHostedService<Runner>();

await builder.Build().RunAsync();
