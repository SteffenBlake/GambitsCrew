namespace GambitsCrew.Domain.Operators;

public record NumberLessThanOrEqualsOperator(
    int LTE
) : INumberOperator
{
    public bool Eval(int value) => value <= LTE;
}

