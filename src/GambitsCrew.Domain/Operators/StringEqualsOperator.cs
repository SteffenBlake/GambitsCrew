namespace GambitsCrew.Domain.Operators;

public record StringEqualsOperator(
    string EQ
) : IStringOperator
{
    public bool Eval(string value) => value == EQ;
}

