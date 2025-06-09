using CommandLine;

namespace GambitsCrew.CLI.NewCrew;

[Verb(name: "new-crew", HelpText = HelpText)]
public class NewCrewOptions 
{
    private const string HelpText = 
@"
Scaffolds a new crew member by name
";

    [Option("n", Required = true, HelpText = "The name for the new crew member")]
    public required string Name { get; set; }
}
