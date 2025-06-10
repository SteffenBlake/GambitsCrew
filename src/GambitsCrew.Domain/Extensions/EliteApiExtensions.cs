using EliteMMO.API;

namespace GambitsCrew.Domain.Extensions;

public record AbilityInfo(int Index, int Id);

public static class EliteApiExtensions 
{
    public static IEnumerable<EliteAPI.EntityEntry> AllianceEntities(this IEliteAPI api) =>
        api.AllianceMembers.Select(a => api.GetEntity((int)a.TargetIndex)!);

    public static IEnumerable<EliteAPI.PartyMember> PartyMembers(this IEliteAPI api) => 
        api.AllianceMembers.Where(a => a.Index < 6); 

    public static IEnumerable<EliteAPI.EntityEntry> PartyEntities(this IEliteAPI api) =>
        api.PartyMembers()
            .Select(m => api.GetEntity((int)m.TargetIndex)!);

    public static EliteAPI.PartyMember? PartyLeader(this IEliteAPI api) =>
        api.PartyMembers()
            .Where(p => p.IsPartyLeader())
            .FirstOrDefault();

    public static EliteAPI.EntityEntry? PartyLeaderEntity(this IEliteAPI api)
    {
        var leader = api.PartyLeader();
        if (leader == null)
        {
            return null;
        }
        return api.GetEntity((int)leader.TargetIndex);
    }
       
    public static EliteAPI.PartyMember? AllianceLeader(this IEliteAPI api) =>
        api.AllianceMembers
            .Where(p => p.IsPartyLeader())
            .FirstOrDefault();

    public static EliteAPI.EntityEntry? AllianceLeaderEntity(this IEliteAPI api)
    {
        var leader = api.AllianceLeader();
        if (leader == null)
        {
            return null;
        }
        return api.GetEntity((int)leader.TargetIndex);
    }

    public static EliteAPI.EntityEntry? TargetEntity(this IEliteAPI api)
    {
        var target = api.Target;
        if (target == null)
        {
            return null;
        }
        return api.GetEntity((int)target.TargetIndex);
    }


    public static TargetType EntityTargetType(this IEliteAPI api, EliteAPI.EntityEntry entity)
    {
        var status = (EntityStatus)entity.Status;

        if (status.HasFlag(EntityStatus.Dead))
        {
            return TargetType.CorpseOnly;
        }

        foreach(var member in api.AllianceMembers)
        {
            if (entity.TargetID != member.TargetIndex)
            {
                continue;
            }
            return member.Index switch
            {
                0 => TargetType.Self,
                < 6 => TargetType.PartyMember,
                _ => TargetType.AllianceMember
            };
        }

        var type = (EntityTypes)entity.Type;
        if (type.HasFlag(EntityTypes.Player))
        {
            return TargetType.Player;
        }
        if (type.HasFlag(EntityTypes.Npc2))
        {
            return TargetType.Enemy;
        }

        return TargetType.Unknown;
    }

    public static IEnumerable<EliteAPI.EntityEntry> ScanEnemies(this IEliteAPI api)
    {
        return Enumerable.Range(0, 1024)
            .Select(api.GetEntity)
            .Where(e => 
                e != null && 
                e.Distance < 50 &&
                e.Exists() && 
                !((EntityStatus)e.Status).HasFlag(EntityStatus.Dead)
            )
            .Cast<EliteAPI.EntityEntry>();
    }

    public static EliteAPI.EntityEntry? BattleTarget(this IEliteAPI api)
    {
        var alliance = api.AllianceEntities().OrderBy(a => a.Distance).ToList();
        var claimIds = alliance.Select(a => a.ServerID).ToHashSet();

        // Check enemies our alliance are targeting first as a shortcut
        foreach(var member in alliance)
        {
            if (member.TargetingIndex <= 0 || member.TargetingIndex >= 1025)
            {
                continue;
            }
            var targetEntity = api.GetEntity(member.TargetingIndex);
            if (targetEntity == null)
            {
                continue;
            }
            if (claimIds.Contains(targetEntity.TargetingIndex))
            {
                return targetEntity;
            }
        }

        // Try scanning nearby enemies, otherwise no battle target was found
        return api.ScanEnemies()
            .Where(e => claimIds.Contains(e.TargetingIndex))
            .OrderBy(e => e.Distance)
            .FirstOrDefault();
    }

    public static IEnumerable<EliteAPI.InventoryItem> GetInventory(this IEliteAPI api)
    {
        return Enumerable.Range(0, 80)
            .Select(api.GetInventoryItem)
            .Where(i => i != null)
            .Cast<EliteAPI.InventoryItem>();
    }

    public static IEnumerable<AbilityInfo> GetAbilityInfos(this IEliteAPI api)
    {
        return Enumerable.Range(0, 32)
            .Select(i => new AbilityInfo(i, api.GetAbilityId(i)))
            .Where(a => a.Id > 0);
    }

    public static bool PlayerHasAbility(this IEliteAPI api, EliteAPI.IAbility ability)
    {
        return api.PlayerHasAbility(ability.ID);
    }

    public static EliteAPI.IAbility GetJobAbility(this IEliteAPI api, string name)
    {
        var ability = api.GetAbility(name);
        var type = (AbilityType)ability.Type;
        if (type.HasFlag(AbilityType.Weaponskill))
        {
            throw new ArgumentException($"Tried to access WS '{name}; as a Job Ability.");
        }
        if (type.HasFlag(AbilityType.Pet))
        {
            throw new ArgumentException($"Tried to access PetSkill '{name}' as a Job Ability.");
        }

        return ability;
    }

    public static int? GetRecast(this IEliteAPI api, EliteAPI.IAbility ability)
    {
        var match = api.GetAbilityInfos()
            .FirstOrDefault(a => a.Id == ability.TimerID);

        if (match == null)
        {
            return null;
        }

        return api.GetAbilityRecast(match.Index);
    }

    public static int GetRecast(this IEliteAPI api, EliteAPI.ISpell spell)
    {
        return api.GetSpellRecast(spell.Index);
    }

    public static EliteAPI.IAbility GetWeaponskill(this IEliteAPI api, string name)
    {
        var ability = api.GetAbility(name);
        var type = (AbilityType)ability.Type;
        if (!type.HasFlag(AbilityType.Weaponskill))
        {
            throw new ArgumentException($"'{name}' is not a Weaponskill.");
        }

        return ability;
    }

    public static EliteAPI.IAbility GetPetSkill(this IEliteAPI api, string name)
    {
        var ability = api.GetAbility(name);
        var type = (AbilityType)ability.Type;
        if (!type.HasFlag(AbilityType.Pet))
        {
            throw new ArgumentException($"'{name}' is not a Pet Skill.");
        }

        return ability;
    }

    public static bool TargetsOverlap(
        this IEliteAPI api,
        EliteAPI.EntityEntry entity, 
        TargetType validTargets
    )
    {
        var targetType = api.EntityTargetType(entity);
        return (targetType & validTargets) > 0;
    }

    public static async Task WaitForPlayerNotBusy(
        this IEliteAPI api, CancellationToken cancellationToken
    )
    {
        do
        {
            await Task.Delay(500, cancellationToken);
        } while (api.PlayerEntity.IsBusy());
    }
}
