using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Commands;

public record ExecuteCommand(
    string Execute
) : ICommand
{

    public Task<bool> TryInvokeAsync(CrewContext ctx, CancellationToken cancellationToken)
    {
        ctx.Api.ThirdParty.SendString(Execute);
        return Task.FromResult(true);
    }
}
