using EliteMMO.API;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record FollowCommandInfo(
    int Min = 3,
    int Max = 5,
    string? Target = null,
    int? Wait = null
);

public record FollowCommand(
    FollowCommandInfo Follow
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = 
            TargetType.Enemy | 
            TargetType.Player | 
            TargetType.PartyMember | 
            TargetType.AllianceMember;

        if (!ctx.EnsureContextualTarget(api, Follow.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for Auto Follow");
        }

        var playerEntity = api.PlayerEntity();
        if (playerEntity == null || !playerEntity.IsIdle())
        {
            return GambitFail.Default;
        }

        await api.AutoFollowAsync(
            (int)ctx.ContextualEntity.TargetID,
            cancellationToken,
            Follow.Min,
            Follow.Max
        );

        // Run option added wait time
        if (Follow.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Follow.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID);
    }
}
