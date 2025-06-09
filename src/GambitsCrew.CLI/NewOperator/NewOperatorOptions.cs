using CommandLine;

namespace GambitsCrew.CLI.NewOperator;

[Verb(name: "new-operator", HelpText = HelpText)]
public class NewOperatorOptions 
{
    private const string HelpText = 
@"
Scaffolds a new operator by name
";

    [Option("n", Required = true, HelpText = "The name for the new operator")]
    public required string Name { get; set; }
}
