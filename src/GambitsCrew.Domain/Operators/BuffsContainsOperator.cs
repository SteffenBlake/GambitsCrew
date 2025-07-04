namespace GambitsCrew.Domain.Operators;

public record BuffsContainsOperator(
    short Contains
) : IBuffsOperator
{
    public bool Eval(short[] buffs)
    {
        return buffs.Contains(Contains);
    }
}
