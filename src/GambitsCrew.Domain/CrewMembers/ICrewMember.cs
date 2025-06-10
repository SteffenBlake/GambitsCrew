using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.CrewMembers;

public interface ICrewMember
{
    string Name { get; init; }
    List<IGambit> Gambits { get; init; }
    Task RunAsync(IEliteAPI api, CancellationToken cancellationToken);
}

