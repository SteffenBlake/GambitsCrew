using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record TargetCommandInfo(
    int? Wait = null
);

public record TargetCommand(
    TargetCommandInfo Target
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        if (!ctx.EnsureContextualTarget(api, null, null))
        {
            throw new InvalidOperationException($"No target inferred for Set Target'");
        }

        api.SetTarget((int)ctx.ContextualEntity.TargetID);

        // Run option added wait time
        if (Target.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Target.Wait.Value), cancellationToken);
        }

        return GambitSuccess.Hashed(_id, ctx.ContextualEntity.TargetID);
    }
}
