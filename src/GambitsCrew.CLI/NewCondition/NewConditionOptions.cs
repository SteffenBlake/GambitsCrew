using CommandLine;

namespace GambitsCrew.CLI.NewCondition;

[Verb(name: "new-condition", HelpText = HelpText)]
public class NewConditionOptions 
{
    private const string HelpText = 
@"
Scaffolds a new Condition by name
";

    [Option("n", Required = true, HelpText = "The name for the new condition")]
    public required string Name { get; set; }
}
