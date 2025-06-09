namespace GambitsCrew.CLI.NewCommand;

public static class NewCommandCmd
{
    internal static async Task<int> RunAsync(NewCommandOptions options)
    {
        var commands = ExampleFiles.EnsureDirectory("Commands");

        await ExampleFiles.EnsureFileAsync(
            commands, options.Name, ExampleFiles.ExampleCommandText
        );

        return 0;
    }
}
