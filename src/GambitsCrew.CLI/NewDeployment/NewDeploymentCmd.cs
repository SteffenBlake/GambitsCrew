
namespace GambitsCrew.CLI.NewDeployment;

public class NewDeploymentCmd
{
    internal static async Task<int> RunAsync(NewDeploymentOptions options)
    {
        await ExampleFiles.EnsureFileAsync(
            Directory.GetCurrentDirectory(), options.Name, ExampleFiles.ExampleDeploymentText
        );

        return 0;
    }
}
