namespace GambitsCrew.Domain.Operators;

public record NumberEqualsOperator(
    int EQ
) : INumberOperator
{
    public bool Eval(int value) => value == EQ;
}

