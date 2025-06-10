using CommandLine;

namespace GambitsCrew.CLI.Run;

[Verb(name: "run", HelpText = HelpText)]
public class RunOptions 
{
    private const string HelpText = 
@"
Runs a deployment by path 
";

    [Value(0)]
    public required string Path { get; set; }
}
