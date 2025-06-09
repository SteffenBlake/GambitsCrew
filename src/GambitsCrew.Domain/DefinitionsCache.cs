using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain;

public class DefinitionsCache 
{
    public Dictionary<string, ICondition> Conditions { get; } = [];
    public Dictionary<string, ISelector> Selectors { get; } = [];
    public Dictionary<string, CrewMember> CrewMembers { get; } = [];
}
