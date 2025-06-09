
namespace GambitsCrew.CLI.NewCondition;

public static class NewConditionCmd
{
    internal static async Task<int> RunAsync(NewConditionOptions options)
    {
        var conditions = ExampleFiles.EnsureDirectory("Conditions");

        await ExampleFiles.EnsureFileAsync(
            conditions, options.Name, ExampleFiles.ExampleConditionText
        );

        return 0;
    }
}
