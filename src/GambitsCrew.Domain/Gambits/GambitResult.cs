namespace GambitsCrew.Domain.Gambits;

public interface IGambitResult;

public record struct GambitSuccess(int Key) : IGambitResult
{
    public static GambitSuccess Hashed<T1>(
        T1 a
    )
    {
        return new GambitSuccess(HashCode.Combine(a));
    }

    public static GambitSuccess Hashed<T1, T2>(
        T1 a, T2 b
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b));
    }

    public static GambitSuccess Hashed<T1, T2, T3>(
        T1 a, T2 b, T3 c
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c));
    }

    public static GambitSuccess Hashed<T1, T2, T3, T4>(
        T1 a, T2 b, T3 c, T4 d
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c, d));
    }

    public static GambitSuccess Hashed<T1, T2, T3, T4, T5>(
        T1 a, T2 b, T3 c, T4 d, T5 e
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c, d, e));
    }

    public static GambitSuccess Hashed<T1, T2, T3, T4, T5, T6>(
        T1 a, T2 b, T3 c, T4 d, T5 e, T6 f
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c, d, e, f));
    }

    public static GambitSuccess Hashed<T1, T2, T3, T4, T5, T6, T7>(
        T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c, d, e, f, g));
    }

    public static GambitSuccess Hashed<T1, T2, T3, T4, T5, T6, T7, T8>(
        T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g, T8 h
    )
    {
        return new GambitSuccess(HashCode.Combine(a, b, c, d, e, f, g, h));
    }
}

public readonly record struct GambitFail() : IGambitResult
{
    public static readonly GambitFail Default = new();
}
