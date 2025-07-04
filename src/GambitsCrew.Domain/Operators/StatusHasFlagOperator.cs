using EliteMMO.API;

namespace GambitsCrew.Domain.Operators;

public record StatusHasFlagOperator(EntityStatus HasFlag) : IStatusOperator
{
    public bool Eval(EntityStatus value) => value.HasFlag(HasFlag);
}

