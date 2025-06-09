using System.Text;
using GambitsCrew.Domain;

namespace GambitsCrew.IntegrationTests.Mocks;

public class MockFileProviderService : IFileProviderService
{
    public Dictionary<string, string> Commands { get; } = [];
    public Stream GetCommand(string name) => ToStream(Commands[name]);

    public Dictionary<string, string> Conditions { get; } = [];
    public Stream GetCondition(string name) => ToStream(Conditions[name]);

    public Dictionary<string, string> CrewMembers { get; } = [];
    public Stream GetCrewMember(string name) => ToStream(CrewMembers[name]);

    public Dictionary<string, string> Deployments { get; } = [];
    public Stream GetDeployment(string name) => ToStream(Deployments[name]);

    public Dictionary<string, string> Gambits { get; } = [];
    public Stream GetGambit(string name) => ToStream(Gambits[name]);

    public Dictionary<string, string> Operators { get; } = [];
    public Stream GetOperator(string name) => ToStream(Operators[name]);

    public Dictionary<string, string> Selectors { get; } = [];
    public Stream GetSelector(string name) => ToStream(Selectors[name]);

    private static MemoryStream ToStream(string content) =>
        new(Encoding.UTF8.GetBytes(content));
}
