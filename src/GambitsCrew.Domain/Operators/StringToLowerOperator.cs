namespace GambitsCrew.Domain.Operators;

public record StringToLowerOperator(
    IStringOperator Lower
) : IStringOperator
{
    public bool Eval(string value) => Lower.Eval(value.ToLower());
}

