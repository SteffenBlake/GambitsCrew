using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public interface ISelector
{
    bool Eval(CrewContext ctx);
}
