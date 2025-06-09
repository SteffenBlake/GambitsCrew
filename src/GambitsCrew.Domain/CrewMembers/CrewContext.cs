using System.Diagnostics.CodeAnalysis;
using EliteMMO.API;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.CrewMembers;

public class CrewContext(EliteAPI api)
{
    public EliteAPI Api => api;

    [field: AllowNull]
    public EliteAPI.PlayerTools Player => field ??= api.Player;

    [field: AllowNull]
    public EliteAPI.XiEntity PlayerEntity => field ??= api.Entity.GetLocalPlayer();

    [field: AllowNull]
    public Dictionary<int, int> AbilityIdsIndexed => field ??= api.Recast.GetAbilityIds()
        .Index()
        .ToDictionary(kvp => kvp.Item, kvp => kvp.Index);

    [field: AllowNull]
    private List<EliteAPI.PartyMember> AllianceMembers => field ??= [..
        api.Party
        .GetPartyMembers()
        .Where(p => p.IsActive())
    ];

    [field: AllowNull]
    private List<EliteAPI.PartyMember> PartyMembers => field ??= [..
        AllianceMembers
        .Where(p => p.IsParty())
    ];

    [field: AllowNull]
    public List<EliteAPI.XiEntity> Alliance => field ??= [..
        AllianceMembers
        .Select(a => api.Entity.GetEntity((int)a.TargetIndex))
        .OrderBy(a => a.Distance)
    ];

    [field: AllowNull]
    public HashSet<uint> AllianceServerIDs => field ??= [..
        Alliance
        .Select(a => a.ServerID)
    ];

    [field: AllowNull]
    public List<EliteAPI.XiEntity> Party => field ??= [..
        PartyMembers
        .Select(p => api.Entity.GetEntity((int)p.TargetIndex))
        .OrderBy(p => p.Distance)
    ];

    [field: AllowNull]
    private HashSet<uint> PartyServerIds => field ??= [..
        Party
        .Select(p => p.ServerID)
    ];

    [field: AllowNull]
    public EliteAPI.XiEntity? PartyLeader => field ??=
        PartyMembers
        .Where(p => p.Index < 6 && p.IsPartyLeader())
        .Select(p => api.Entity.GetEntity((int)p.TargetIndex))
        .FirstOrDefault();

    [field: AllowNull]
    public EliteAPI.XiEntity? AllianceLeader => field ??=
        PartyMembers
        .Where(p => p.Index < 6 && p.IsAllianceLeader())
        .Select(p => api.Entity.GetEntity((int)p.TargetIndex))
        .FirstOrDefault();


    [field: AllowNull]
    public EliteAPI.XiEntity? Pet => field ??=
        api.Player.PetIndex <= 0 ? null :
        api.Player.Pet;

    [field: AllowNull]
    public EliteAPI.XiEntity? Target => field ??= api.Entity.GetEntity(
            (int)api.Target
            .GetTargetInfo()
            .TargetIndex
        );

    [field: AllowNull]
    public EliteAPI.XiEntity? BattleTarget
    {
        get
        {
            if (field != null)
            {
                return field;
            }

            foreach (var a in Alliance)
            {
                if (a.TargetingIndex <= 0)
                {
                    continue;
                }
                var target = Api.Entity.GetEntity(a.TargetingIndex);
                if (target.ClaimID <= 0)
                {
                    continue;
                }
                if (AllianceServerIDs.Contains(target.ClaimID))
                {
                    return field = target;
                }
            }

            for (var index = 0; index <= 1024; index++)
            {
                var target = Api.Entity.GetEntity(index);
                if (target.ClaimID <= 0)
                {
                    continue;
                }
                if (AllianceServerIDs.Contains(target.ClaimID))
                {
                    return field = target;
                }
            }

            return null;
        }
    }

    public EliteAPI.XiEntity? ContextualEntity { get; set; } = null;

    public TargetType? ContextualTargetType
    {
        get {
            if (ContextualEntity == null)
            {
                return null;
            }

            var status = (EntityStatus)ContextualEntity.Status;

            if (status.HasFlag(EntityStatus.Dead))
            {
                return TargetType.CorpseOnly;
            }

            if (ContextualEntity.ServerID == PlayerEntity.ServerID)
            {
                return TargetType.Self;
            }
            if (PartyServerIds.Contains(ContextualEntity.ServerID))
            {
                return TargetType.PartyMember;
            }
            if (AllianceServerIDs.Contains(ContextualEntity.ServerID))
            {
                return TargetType.AllianceMember;
            }
            var type = (EntityTypes)ContextualEntity.Type;
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
    }

    public string? ContextualTarget { get; set; } = null;

    [field: AllowNull]
    public HashSet<ushort> InventoryItemIds => field ??= [.. 
        Enumerable.Range(0, 80)
            .Select(i => api.Inventory.GetContainerItem(0, i))
            .Where(i => i != null)
            .Select(i => i.Id)
    ];

    public bool EnsureContextualTarget(string? targetString, TargetType? targetType)
    {
        if (!string.IsNullOrEmpty(targetString))
        {
            if (TryInferTargetFromString(targetString))
            {
                return true;
            }
        }

        if (ContextualTarget != null)
        {
            if (targetType.HasValue && (targetType.Value & ContextualTargetType) > 0)
            {
                return true;
            }
            // If we hit here, it means we had a guessed target but its not actually valid
            // In which case, we will continue trying to best guess the target
            // based on other context
        }

        if (targetType.HasValue)
        {
            if (TryInferTargetFromType(targetType.Value))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryInferTargetFromString(string target)
    {
        var entity = target switch 
        {
            "<me>" => PlayerEntity,
            "<t>" => Target,
            "<ldr>" => PartyLeader ?? PlayerEntity,
            "<bt>" => BattleTarget,
            _ => Alliance.SingleOrDefault(a => a.Name == target)
        };

        if (entity == null)
        {
            return false;
        }

        ContextualEntity = entity;
        ContextualTarget = target;
        return true;
    }

    public bool TryInferTargetFromType(TargetType target)
    {
        if (target == TargetType.Self)
        {
            ContextualEntity = PlayerEntity;
            ContextualTarget = "<me>";
            return true;
        }
        if (Target == null)
        {
            return false;
        }

        var currentTargetType = (TargetType)Target.Type;

        if ((target & currentTargetType) > 0)
        {
            ContextualEntity = Target;
            ContextualTarget = "<t>";
            return true;
        }

        return false;
    }
}
