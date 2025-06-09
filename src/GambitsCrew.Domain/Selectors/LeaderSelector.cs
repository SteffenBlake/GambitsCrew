using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record LeaderSelector(
    List<ICondition> L 
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        if (ctx.PartyLeader == null)
        {
            return false;
        }
        ctx.ContextualEntity = ctx.PartyLeader;
        ctx.ContextualTarget = ctx.PartyLeader.Name;
        return L.All(condition => condition.Eval(ctx));
    }
}
