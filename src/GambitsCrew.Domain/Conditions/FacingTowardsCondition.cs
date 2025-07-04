using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Conditions;

public record FacingTowardsCondition(
    bool FacingTowards
) : ICondition
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        if (ctx.ContextualEntity == null)
        {
            return false;
        }

        var player = api.PlayerEntity();
        if (player == null)
        {
            return false;
        }

        var isFacing = player.IsFacing(ctx.ContextualEntity, tolerance: 0.10f);
        return isFacing == FacingTowards;
    }
}

