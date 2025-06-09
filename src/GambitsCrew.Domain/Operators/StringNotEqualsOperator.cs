namespace GambitsCrew.Domain.Operators;

public record StringNotEqualsOperator(
    string NE
) : IStringOperator
{
    public bool Eval(string value) => value != NE;
}

