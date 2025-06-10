namespace GambitsCrew.CLI;

public static class ExampleFiles 
{
    public static string EnsureDirectory(string dirName)
    {
        var dir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
        Directory.CreateDirectory(dir);

        return dir;
    }

    public static async Task EnsureFileAsync(string dir, string name, string content)
    {
        var path = Path.Combine(dir, $"{name}.json");
        await File.WriteAllTextAsync(path, content);
    }


    public const string ExampleCommandText = 
"""
{
    "cast": { "name": "Cure IV" }
}
""";

    public const string ExampleConditionText = 
"""
{
    "hpp": [
        { "gt": 25 },
        { "lt": 75 }
    ]
}
""";

    public const string ExampleCrewText = 
"""
{
    "name": "ExampleName",
    "gambits": ["ExampleGambit"]
}
""";

    public const string ExampleDeploymentText = 
"""
{
    "crew": ["ExampleCrew"]
}
""";

    public const string ExampleGambitText = 
"""
{
    "when": "ExampleSelector",
    "do": "ExampleCommand"
}
""";

    public const string ExampleOperatorText = 
"""
{
    "eq": 10
}
""";

    public const string ExampleSelectorText = 
"""
{
    "and": [
        { "l": [ "ExampleCondition" ] },
        { "p": [ "ExampleCondition" ] }
    ]
}
""";

}
