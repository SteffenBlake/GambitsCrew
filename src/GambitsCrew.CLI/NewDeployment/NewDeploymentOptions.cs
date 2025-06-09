using CommandLine;

namespace GambitsCrew.CLI.NewDeployment;

[Verb(name: "new-deployment", HelpText = HelpText)]
public class NewDeploymentOptions 
{
    private const string HelpText = 
@"
Scaffolds a new deployment by name
";

    [Option("n", Required = true, HelpText = "The name for the new deployment")]
    public required string Name { get; set; }
}
