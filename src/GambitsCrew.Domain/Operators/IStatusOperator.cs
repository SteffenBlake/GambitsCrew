using EliteMMO.API;

namespace GambitsCrew.Domain.Operators;

public interface IStatusOperator
{
    bool Eval(EntityStatus value);
}
