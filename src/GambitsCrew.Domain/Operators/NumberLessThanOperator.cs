namespace GambitsCrew.Domain.Operators;

public record NumberLessThanOperator(
    int LT
) : INumberOperator
{
    public bool Eval(int value) => value < LT;
}

