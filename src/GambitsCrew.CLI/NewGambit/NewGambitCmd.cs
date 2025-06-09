
namespace GambitsCrew.CLI.NewGambit;

public static class NewGambitCmd
{
    internal static async Task<int> RunAsync(NewGambitOptions options)
    {
        var gambits = ExampleFiles.EnsureDirectory("Gambits");

        await ExampleFiles.EnsureFileAsync(
            gambits, options.Name, ExampleFiles.ExampleGambitText
        );

        return 0;
    }
}
