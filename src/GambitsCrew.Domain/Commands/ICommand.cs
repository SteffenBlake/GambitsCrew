using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Commands;

public interface ICommand 
{
    Task<bool> TryInvokeAsync(CrewContext ctx, CancellationToken cancellationToken);
}
