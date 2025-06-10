using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Conditions;

public interface ICondition 
{
    bool Eval(GambitContext ctx);
}
