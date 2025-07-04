using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Conditions;

public record ClaimedCondition(
    bool Claimed
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }

        return (ctx.ContextualEntity.ClaimID > 0) == Claimed;
    }
}

