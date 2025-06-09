using CommandLine;

namespace GambitsCrew.CLI.Init;


[Verb(name: "init", HelpText = HelpText)]
public class InitOptions 
{
    private const string HelpText = 
@"
Initializes a new GambitsCrew project from the default template
";
}
