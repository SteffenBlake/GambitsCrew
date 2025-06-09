using GambitsCrew.Domain;

namespace GambitsCrew.CLI;

public class ProjectFileProviderService(
    string projectDirectoryRoot
) : IFileProviderService
{
    public Stream GetCommand(string name) => GetFile("Commands", name);

    public Stream GetCondition(string name) => GetFile("Conditions", name);

    public Stream GetCrewMember(string name) => GetFile("Crew", name);

    public Stream GetDeployment(string name) => GetFile("Deployments", name);

    public Stream GetGambit(string name) => GetFile("Gambits", name);

    public Stream GetOperator(string name) => GetFile("Operators", name);

    public Stream GetSelector(string name) => GetFile("Selectors", name);

    private FileStream GetFile(string dir, string name)
    {
        var path = Path.Combine(projectDirectoryRoot, dir, $"{name}.json");
        return File.OpenRead(path);
    }
}
