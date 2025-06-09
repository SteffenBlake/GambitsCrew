namespace GambitsCrew.Domain.Operators;

public record StringContainsOperator(
    string Contains
) : IStringOperator
{
    public bool Eval(string value) => value.Contains(Contains);
}

