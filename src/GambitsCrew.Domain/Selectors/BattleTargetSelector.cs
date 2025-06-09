using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record BattleTargetSelector(
    List<ICondition> BT 
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        if (ctx.BattleTarget == null)
        {
            return false;
        }

        ctx.ContextualTarget = "<bt>";
       
        ctx.ContextualEntity = ctx.BattleTarget;

        return BT.All(condition => condition.Eval(ctx)); 
    }
}
