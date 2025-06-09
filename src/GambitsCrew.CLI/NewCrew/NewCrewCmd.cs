
namespace GambitsCrew.CLI.NewCrew;

public static class NewCrewCmd
{
    internal static async Task<int> RunAsync(NewCrewOptions options)
    {
        var crew = ExampleFiles.EnsureDirectory("Crew");

        await ExampleFiles.EnsureFileAsync(
            crew, options.Name, ExampleFiles.ExampleCrewText
        );

        return 0;
    }
}
