using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Conditions;

public interface ICondition 
{
    bool Eval(CrewContext ctx);
}
