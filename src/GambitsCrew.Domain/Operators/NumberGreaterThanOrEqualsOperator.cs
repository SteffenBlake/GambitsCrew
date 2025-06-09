namespace GambitsCrew.Domain.Operators;

public record NumberGreaterThanOrEqualsOperator(
    int GTE
) : INumberOperator
{
    public bool Eval(int value) => value >= GTE;
}

