using EliteMMO.API;

namespace GambitsCrew.Domain.Extensions;

public static class XiEntityExtensions 
{
    public static bool Exists(this EliteAPI.XiEntity entity) => 
        entity.TargetID > 0;

    public static bool IsBusy(this EliteAPI.XiEntity entity) =>
        entity.ActionTimer1 > 0;
}
