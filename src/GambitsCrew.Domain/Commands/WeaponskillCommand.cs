using EliteMMO.API;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Extensions;

namespace GambitsCrew.Domain.Commands;

public record WeaponskillCommandInfo(
    string Name,
    int TP = 1000,
    int? Wait = null
);

public record WeaponskillCommand(
    WeaponskillCommandInfo WS
) : ICommand
{
    public async Task<bool> TryInvokeAsync(
        CrewContext ctx, CancellationToken cancellationToken
    )
    {
        if (ctx.ContextualEntity == null)
        {
            throw new InvalidOperationException("Entity didnt get set?");
        }

        var ws = ctx.Api.Resources.GetAbility(WS.Name, 0) ??
            throw new ArgumentException($"No known Weaponskill: '{WS.Name}'");

        var type = (AbilityType)ws.Type;
        if (!type.HasFlag(AbilityType.Weaponskill))
        {
            throw new ArgumentException($"'{WS.Name}' is not a Weaponskill");
        }

        // Check targeting enemy
        if (!ctx.ContextualTargetType!.Value.HasFlag(TargetType.Enemy))
        {
            return false;
        }

        // Check busy
        if (ctx.PlayerEntity.IsBusy())
        {
            return false;
        }

        // Check TP
        if (ctx.Player.TP < WS.TP)
        {
            return false;
        }

        // Check too far
        if (ctx.ContextualEntity.Distance > 5.9f)
        {
            return false;
        }

        // Check can use WS
        if (!ctx.Player.HasWeaponSkill(ws.ID))
        {
            return false;
        }

        ctx.Api.ThirdParty.SendString($"/ws {WS.Name} {ctx.ContextualTarget}");

        // Wait for busy to finish
        do
        {
            await Task.Delay(100, cancellationToken);
        } while (ctx.PlayerEntity.IsBusy());

        // Run option added wait time
        if (WS.Wait.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(WS.Wait.Value), cancellationToken);
        }

        return true;
    }
}


