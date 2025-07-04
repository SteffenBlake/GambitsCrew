using EliteMMO.API;
using GambitsCrew.Domain.Extensions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record AssistCommandInfo(
    string? Target = null,
    int? Wait = null
);

public record AssistCommand(
    AssistCommandInfo Assist
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var validTargets = TargetType.PartyMember | TargetType.PartyMember;
        if (!ctx.EnsureContextualTarget(api, Assist.Target, validTargets))
        {
            throw new InvalidOperationException("Could not infer a valid target for Assist");
        }
        var alliance = api.AllianceEntities();

        // Find the member to assist
        var member = alliance.SingleOrDefault(a => a.Name == ctx.ContextualTarget);
        if (member == null)
        {
            return GambitFail.Default;
        }

        var playerEntity = alliance.First();

        // Check if we need to do an assist
        var currentTargetId = playerEntity.TargetingIndex;
        var assistTargetId = member.TargetingIndex;
        if (assistTargetId <= 0 || assistTargetId == currentTargetId)
        {
            return GambitFail.Default;
        }

        // Check if too far away
        var assistTargetEntity = api.GetEntity(assistTargetId);
        if (assistTargetEntity == null)
        {
            return GambitFail.Default;
        }

        if (assistTargetEntity.Distance <= 0 || assistTargetEntity.Distance > 49)
        {
            return GambitFail.Default;
        }

        // Execute
        api.SetTarget(assistTargetId);

        ctx.ContextualEntity = assistTargetEntity;
        ctx.ContextualTarget = "<t>";

        // Run option added wait time
        if (Assist.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Assist.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, assistTargetId);
    }
}
