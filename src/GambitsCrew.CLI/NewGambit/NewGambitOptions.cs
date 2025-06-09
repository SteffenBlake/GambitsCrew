using CommandLine;

namespace GambitsCrew.CLI.NewGambit;

[Verb(name: "new-gambit", HelpText = HelpText)]
public class NewGambitOptions 
{
    private const string HelpText = 
@"
Scaffolds a new gambit by name
";

    [Option("n", Required = true, HelpText = "The name for the new gambit")]
    public required string Name { get; set; }
}
