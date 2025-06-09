namespace GambitsCrew.Domain.Operators;

public record NumberNotEqualsOperator(
    int NE
) : INumberOperator
{
    public bool Eval(int value) => value != NE;
}

