using EliteMMO.API;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record ItemCommandInfo(
    string Name,
    string? Target = null,
    int? Wait = null
);

public record ItemCommand(
    ItemCommandInfo Item
) : ICommand
{
    private readonly Guid _id = Guid.NewGuid();

    public async Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        var item = api.GetItem(Item.Name);

        var flags = (ItemFlagsMask)item.Flags;
        if (!flags.HasFlag(ItemFlagsMask.CanUse))
        {
            throw new ArgumentException($"Item is not useable: '{Item.Name}'");
        }
      
        var playerEntity = api.PlayerEntity();
        // Check busy
        if (playerEntity == null ||  playerEntity.IsBusy())
        {
            return GambitFail.Default;
        }

        // Check idle
        if (!playerEntity.IsIdle())
        {
            return GambitFail.Default;
        }

        // Check inventory
        if (!api.GetInventory().Any(i => i.Id == (ushort)item.ItemID))
        {
            return GambitFail.Default;
        }

        var validTargets = (TargetType)item.ValidTargets;
        if (!ctx.EnsureContextualTarget(api, Item.Target, validTargets))
        {
            throw new InvalidOperationException($"No target inferred for item '{Item.Name}'");
        }

        if (!api.TargetsOverlap(ctx.ContextualEntity, validTargets))
        {
            throw new InvalidOperationException(
                $"Tried to use Item '{item.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        api.SendString($"/item \"{Item.Name}\" <me>");
        
        // Wait for busy to finish
        await api.WaitForPlayerNotBusy(cancellationToken);
        
        // Run option added wait time
        if (Item.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Item.Wait.Value), cancellationToken);
        }
        
        return GambitSuccess.Hashed(_id, item.ItemID);
    }
}
