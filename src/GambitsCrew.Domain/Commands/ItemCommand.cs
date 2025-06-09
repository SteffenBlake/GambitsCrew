using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record ItemCommandInfo(
    string Name,
    int? Wait = null
);

public record ItemCommand(
    ItemCommandInfo Item
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        ctx.ContextualEntity ??= ctx.PlayerEntity;

        var item = ctx.Api.Resources.GetItem(Item.Name, 0) ??
            throw new ArgumentException($"No known Item: '{Item.Name}'");

        var flags = (ItemFlagsMask)item.Flags;
        if (!flags.HasFlag(ItemFlagsMask.CanUse))
        {
            throw new ArgumentException($"Item is not useable: '{Item.Name}'");
        }
       
        // Check busy
        if (ctx.PlayerEntity.IsBusy())
        {
            return false;
        }

        // Check inventory
        if (!ctx.InventoryItemIds.Contains((ushort)item.ItemID))
        {
            return false;
        }

        var validTargets = (TargetType)item.ValidTargets;
        if ((validTargets & ctx.ContextualTargetType) <= 0)
        {
            throw new InvalidOperationException(
                $"Tried to use Item '{item.Name}' on target '{ctx.ContextualEntity!.Name}'"
            );
        }

        ctx.Api.ThirdParty.SendString($"/item {Item.Name} <me>");
        
        // Wait for busy to finish
        do
        {
            await Task.Delay(100, cancellationToken);
        } while (ctx.PlayerEntity.IsBusy());
        
        // Run option added wait time
        if (Item.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Item.Wait.Value), cancellationToken);
        }
        
        return true;
    }
}
