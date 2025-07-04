using EliteMMO.API;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain;

public interface IEliteAPI
{
    IEnumerable<EliteAPI.PartyMember> AllianceMembers { get; }

    EliteAPI.EntityEntry? GetEntity(int index);

    EliteAPI.TargetInfo? Target { get; }

    void SetTarget(int index);

    void SetPlayerHeading(float heading);

    EliteAPI.IItem GetItem(string name);

    EliteAPI.InventoryItem? GetInventoryItem(int index);


    int GetAbilityId(int index);

    EliteAPI.IAbility GetAbility(string name);

    bool PlayerHasAbility(ushort id);

    int GetAbilityRecast(int index);


    EliteAPI.ISpell GetSpell(string name);

    bool PlayerHasSpell(uint id);

    int GetSpellRecast(int index);

    bool PlayerHasWeaponskill(uint id);

    bool SetAutoFollowCoords(float fX, float fY, float fZ);
    void CancelAutoFollow();

    void SendString(string v);
    short[] PlayerBuffs();
}

public class EliteApiWrapper(EliteAPI api) : IEliteAPI
{
    public EliteAPI.PartyMember PlayerMember => api.Party.GetPartyMember(0);

    public EliteAPI.EntityEntry PlayerEntity => api.Entity.GetStaticEntity(
        api.Entity.LocalPlayerIndex
    );

    public EliteAPI.EntityEntry? PetEntity
    {
        get
        {
            var pet = api.Entity.GetStaticEntity(api.Player.PetIndex);
            if (pet.TargetID <= 0)
            {
                return null;
            }
            return pet;
        }
    }

    public IEnumerable<EliteAPI.PartyMember> AllianceMembers =>
        api.Party.GetPartyMembers().Where(p => p.IsActive());

    public EliteAPI.EntityEntry? GetEntity(int index) 
    {
        var entity = api.Entity.GetStaticEntity(index);
        if (entity?.TargetID > 0)
        {
            return entity;
        }
        return null;
    }

    public EliteAPI.TargetInfo? Target
    {
        get
        {
            var target = api.Target.GetTargetInfo();
            if (target.TargetIndex <= 0)
            {
                return null;
            }
            return target;
        }
    }

    public void SetTarget(int index)
    {
        api.Target.SetTarget(index);
    }

    public void SetPlayerHeading(float heading)
    {
        api.Entity.GetLocalPlayer().H = heading;
    }

    public short[] PlayerBuffs() => api.Player.Buffs;

    public EliteAPI.IItem GetItem(string name)
    {
        var item = api.Resources.GetItem(name, 0);
        if (item?.ItemID > 0)
        {
            return item;
        }

        throw new ArgumentException($"No known item with name '{name}'");
    }

    public EliteAPI.InventoryItem? GetInventoryItem(int index)
    {
        var item = api.Inventory.GetContainerItem(0, index);
        if (item?.Id > 0)
        {
            return item;
        }
        return null;
    }


    public int GetAbilityId(int index)
    {
        return api.Recast.GetAbilityId(index);
    }

    public EliteAPI.IAbility GetAbility(string name)
    {
        var ability = api.Resources.GetAbility(name, 0);
        if (ability?.ID >= 0)
        {
            return ability;
        }

        throw new ArgumentException($"No known ability with name '{name}'");
    }

    public bool PlayerHasAbility(ushort id)
    {
        return api.Player.HasAbility(id);
    }

    public int GetAbilityRecast(int index)
    {
        return api.Recast.GetAbilityRecast(index);
    }


    public EliteAPI.ISpell GetSpell(string name)
    {
        var spell = api.Resources.GetSpell(name, 0);
        if (spell?.ID >= 0)
        {
            return spell;
        }

        throw new ArgumentException($"No known spell with name '{name}'");
    }

    public bool PlayerHasSpell(uint id)
    {
        return api.Player.HasSpell(id);
    }

    public int GetSpellRecast(int index)
    {
        return api.Recast.GetSpellRecast(index);
    }

    public bool PlayerHasWeaponskill(uint id)
    {
        return api.Player.HasWeaponSkill(id);
    }

    public bool SetAutoFollowCoords(float fX, float fY, float fZ)
    {
        if(api.AutoFollow.SetAutoFollowCoords(fX, fY, fZ))
        {
            api.AutoFollow.IsAutoFollowing = true;
            return true;
        }

        api.AutoFollow.IsAutoFollowing = false;
        return false;
    }

    public void CancelAutoFollow()
    {
        api.AutoFollow.IsAutoFollowing = false;
    }

    public void SendString(string str)
    {
        api.ThirdParty.SendString(str);
    }
}
