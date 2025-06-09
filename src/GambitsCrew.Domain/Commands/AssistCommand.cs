using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Commands;

public record AssistCommandInfo(
    string? Target = null,
    int? Wait = null
);

public record AssistCommand(
    AssistCommandInfo Assist
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        // Find the member to assist
        var memberName = Assist.Target ?? ctx.ContextualTarget;
        var member = ctx.Alliance.SingleOrDefault(a => a.Name == memberName);
        if (member == null)
        {
            return false;
        }

        // Check if we need to do an assist
        var currentTargetId = ctx.PlayerEntity.TargetingIndex;
        var assistTargetId = member.TargetingIndex;
        if (assistTargetId <= 0 || assistTargetId == currentTargetId)
        {
            return false;
        }

        // Check if too far away
        var assistTargetEntity = ctx.Api.Entity.GetEntity(assistTargetId);
        if (assistTargetEntity.Distance <= 0 || assistTargetEntity.Distance > 49)
        {
            return false;
        }

        // Execute
        ctx.Api.Target.SetTarget(assistTargetId);
        ctx.ContextualEntity = assistTargetEntity;
        ctx.ContextualTarget = "<t>";

        // Run option added wait time
        if (Assist.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Assist.Wait.Value), cancellationToken);
        }

        return true;
    }
}
