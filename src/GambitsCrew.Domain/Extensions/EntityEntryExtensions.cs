using EliteMMO.API;

namespace GambitsCrew.Domain.Extensions;

public static class EntityEntryExtensions 
{
    public static bool Exists(this EliteAPI.EntityEntry entity) => 
        entity.TargetID > 0;

    public static bool IsBusy(this EliteAPI.EntityEntry entity) =>
        entity.ActionTimer1 > 0;
}
