using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record LeaderSelector(
    List<ICondition> L 
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        var leader = api.PartyLeaderEntity();
        if (leader == null)
        {
            return false;
        }
        ctx.ContextualEntity = leader;
        ctx.ContextualTarget = leader.Name;
        return L.All(condition => condition.Eval(ctx, api));
    }
}
