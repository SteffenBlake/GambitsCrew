
namespace GambitsCrew.CLI.NewOperator;

public static class NewOperatorCmd
{
    internal static async Task<int> RunAsync(NewOperatorOptions options)
    {
        var operators = ExampleFiles.EnsureDirectory("Operators");

        await ExampleFiles.EnsureFileAsync(
            operators, options.Name, ExampleFiles.ExampleOperatorText
        );

        return 0;
    }
}
