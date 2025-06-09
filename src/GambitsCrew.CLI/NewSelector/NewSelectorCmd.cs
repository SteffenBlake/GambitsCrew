
namespace GambitsCrew.CLI.NewSelector;

public static class NewSelectorCmd
{
    internal static async Task<int> RunAsync(NewSelectorOptions options)
    {
        var selectors = ExampleFiles.EnsureDirectory("Selectors");

        await ExampleFiles.EnsureFileAsync(
            selectors, options.Name, ExampleFiles.ExampleSelectorText
        );

        return 0;
    }
}
