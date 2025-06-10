using EliteMMO.API;
using GambitsCrew.Domain;

namespace GambitsCrew.IntegrationTests.Mocks;

public class MockEliteApi : IEliteAPI
{
    public EliteAPI.PartyMember PlayerMember { get; } = new();

    public EliteAPI.EntityEntry PlayerEntity { get; } = new();

    public EliteAPI.EntityEntry? PetEntity { get; } = null;

    public EliteAPI.PartyMember[] AllianceMembersRaw = [.. 
        Enumerable.Range(0, 18).Select(_ => new EliteAPI.PartyMember())
    ];

    public IEnumerable<EliteAPI.PartyMember> AllianceMembers => AllianceMembersRaw;

    public EliteAPI.TargetInfo? Target { get; } = null;

    public Dictionary<string, EliteAPI.IAbility> AbilitiesByName { get; } = [];
    public EliteAPI.IAbility GetAbility(string name)
    {
        AbilitiesByName.TryGetValue(name, out var ability);
        return ability ?? new();
    }

    public int[] PlayerAbilityIds { get; } = new int[32];
    public int GetAbilityId(int index) => PlayerAbilityIds[index];

    public int[] PlayerAbilityRecasts { get; } = new int[32];
    public int GetAbilityRecast(int index) => PlayerAbilityRecasts[index];

    public Dictionary<int, EliteAPI.EntityEntry> EntitiesByIndex { get; } = [];
    public EliteAPI.EntityEntry? GetEntity(int index)
    {
        _ = EntitiesByIndex.TryGetValue(index, out var entity);
        return entity;
    }

    public EliteAPI.InventoryItem?[] Inventory { get; } = new EliteAPI.InventoryItem?[80];
    public EliteAPI.InventoryItem? GetInventoryItem(int index) => Inventory[index];

    public Dictionary<string, EliteAPI.IItem> ItemsByName { get; } = [];
    public EliteAPI.IItem GetItem(string name)
    {
        ItemsByName.TryGetValue(name, out var item);
        return item ?? new();
    }

    public Dictionary<string, EliteAPI.ISpell> SpellsByName { get; } = [];
    public EliteAPI.ISpell GetSpell(string name)
    {
        SpellsByName.TryGetValue(name, out var spell);
        return spell ?? new();
    }

    public Dictionary<int, int> SpellRecasts { get; } = [];
    public int GetSpellRecast(int index)
    {
        SpellRecasts.TryGetValue(index, out var recast);
        return recast;
    }

    public bool PlayerHasAbility(ushort id) => PlayerAbilityIds.Contains(id);

    public HashSet<uint> PlayerSpellIds { get; } = [];
    public bool PlayerHasSpell(uint id) => PlayerSpellIds.Contains(id);

    public HashSet<uint> PlayerWeaponskillIds { get; } = [];
    public bool PlayerHasWeaponskill(uint id) => PlayerWeaponskillIds.Contains(id);

    public IReadOnlyList<string> SentStrings => _sentStrings;
    private readonly List<string> _sentStrings = [];
    public void SendString(string str) => _sentStrings.Add(str);

    public IReadOnlyList<int> SetTargets => _setTargets;
    private readonly List<int> _setTargets = [];
    public void SetTarget(int index) => _setTargets.Add(index);
}
