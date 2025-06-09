using CommandLine;

namespace GambitsCrew.CLI.NewSelector;

[Verb(name: "new-selector", HelpText = HelpText)]
public class NewSelectorOptions 
{
    private const string HelpText = 
@"
Scaffolds a new selector by name
";

    [Option("n", Required = true, HelpText = "The name for the new selector")]
    public required string Name { get; set; }
}
