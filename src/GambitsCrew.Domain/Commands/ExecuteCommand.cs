using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Commands;

public record ExecuteCommand(
    string Execute
) : ICommand
{

    private readonly Guid _id = Guid.NewGuid();

    public Task<IGambitResult> TryInvokeAsync(
        GambitContext ctx, IEliteAPI api, CancellationToken cancellationToken
    )
    {
        api.SendString(Execute);

        return Task.FromResult(
            (IGambitResult)GambitSuccess.Hashed(_id)
        );
    }
}
