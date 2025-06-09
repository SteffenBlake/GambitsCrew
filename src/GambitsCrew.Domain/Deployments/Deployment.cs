using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Deployments;

public record Deployment(
    List<CrewMember> Crew
);
