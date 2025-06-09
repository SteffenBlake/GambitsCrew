using EliteMMO.API;

namespace GambitsCrew.Domain.Extensions;

public static class PartyMemberExtensions 
{
    public static bool IsActive(this EliteAPI.PartyMember p) => p.Active > 0;
    public static bool IsParty(this EliteAPI.PartyMember p) => p.Index < 6;
    public static bool IsPartyLeader(this EliteAPI.PartyMember p) => (p.FlagMask & 4) > 0;
    public static bool IsAllianceLeader(this EliteAPI.PartyMember p) => (p.FlagMask & 8) > 0;
}
