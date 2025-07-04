using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.Domain.Conditions;

public record BuffsCondition(
    List<IBuffsOperator> Buffs
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }

        var buffs = api.PlayerBuffs();
        return Buffs.All(condition => condition.Eval(buffs));
    }
}

