

namespace GambitsCrew.CLI.Init;

public static class InitCmd
{
    internal static async Task<int> RunAsync(InitOptions _)
    {
        var commands = ExampleFiles.EnsureDirectory("Commands");
        var conditions = ExampleFiles.EnsureDirectory("Conditions");
        var crew = ExampleFiles.EnsureDirectory("Crew");
        var gambits = ExampleFiles.EnsureDirectory("Gambits");
        var operators = ExampleFiles.EnsureDirectory("Operators");
        var selectors = ExampleFiles.EnsureDirectory("Selectors");

        await ExampleFiles.EnsureFileAsync(commands, "ExampleCommand", ExampleFiles.ExampleCommandText);
        await ExampleFiles.EnsureFileAsync(conditions, "ExampleCondition", ExampleFiles.ExampleConditionText);
        await ExampleFiles.EnsureFileAsync(crew, "ExampleCrewMember", ExampleFiles.ExampleCrewText);
        await ExampleFiles.EnsureFileAsync(gambits, "ExampleGambit", ExampleFiles.ExampleGambitText);
        await ExampleFiles.EnsureFileAsync(operators, "ExampleOperator", ExampleFiles.ExampleOperatorText);
        await ExampleFiles.EnsureFileAsync(selectors, "ExampleSelector", ExampleFiles.ExampleSelectorText);

        var deploymentPath = Path.Combine(
            Directory.GetCurrentDirectory(), "ExampleDeployment.json"
        );
        await File.WriteAllTextAsync(deploymentPath, ExampleFiles.ExampleDeploymentText);

        return 0;
    }
}
