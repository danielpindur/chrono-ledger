namespace ChronoLedger.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable is not null && enumerable.Any();
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable is null || !enumerable.Any();
    }
}