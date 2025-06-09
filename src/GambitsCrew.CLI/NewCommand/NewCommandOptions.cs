using CommandLine;

namespace GambitsCrew.CLI.NewCommand;

[Verb(name: "new-cmd", HelpText = HelpText)]
public class NewCommandOptions 
{
    private const string HelpText = 
@"
Scaffolds a new Command by name
";

    [Option("n", Required = true, HelpText = "The name for the new command")]
    public required string Name { get; set; }
}
