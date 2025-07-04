using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record BattleTargetSelector(
    List<ICondition> BT 
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        var battleTarget = api.BattleTarget();
        if (battleTarget == null)
        {
            return false;
        }

        ctx.ContextualTarget = "<bt>";
        ctx.ContextualEntity = battleTarget;

        return BT.All(condition => condition.Eval(ctx, api)); 
    }
}
