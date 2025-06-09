namespace GambitsCrew.Domain.Operators;

public record NumberGreaterThanOperator(
    int GT
) : INumberOperator
{
    public bool Eval(int value) => value > GT;     
}
