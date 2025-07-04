using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record FaceTowardsCommand(
    bool FaceTowards
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();
    public Task<IGambitResult> TryInvokeAsync(GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken)
    {
        if (ctx.ContextualEntity == null)
        {
            return Task.FromResult((IGambitResult)GambitFail.Default);
        }

        var player = api.PlayerEntity();
        // Fail if we are trying to, for some reason, face towards ourselves
        if (player == null || player.ServerID == ctx.ContextualEntity.ServerID)
        {
            // TODO: Should this throw an exception instead, as it implies
            // We entered an invalid machine state really...
            // This will "silently fail" which maybe is bad UX
            return Task.FromResult((IGambitResult)GambitFail.Default);
        }

        if (FaceTowards)
        {
            api.FaceTowards(player, ctx.ContextualEntity);
        }
        else 
        {
            api.FaceAwayFrom(player, ctx.ContextualEntity);
        }

        return Task.FromResult(
            (IGambitResult)GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID)
        );
    }
}
