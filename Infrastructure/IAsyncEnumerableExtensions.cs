using LanguageExt;
using static LanguageExt.Prelude;

namespace Infrastructure;

internal static class IAsyncEnumerableExtensions
{
    public static Func<Func<ET, ST>, Func<ST, ET, ST>, Task<Option<ST>>> StateCalculator<ST, ET>(this IAsyncEnumerable<ET> history)
    => (creator, aggregator) => history.MatchAsync(
        empty: () => Task.FromResult(Option<ST>.None),
        more: (created, otherEvents)
            => Optional(
                otherEvents.AggregateAsync(creator(created), aggregator)
                .AsTask())
                .Sequence());

    public static async Task<Option<T>> Head<T>(this IAsyncEnumerable<T> source)
    {
        var enumerator = source.GetAsyncEnumerator();
        var isNext = await enumerator.MoveNextAsync();
        return isNext ? enumerator.Current : None;
    }

    public async static Task<R> MatchAsync<T, R>(this IAsyncEnumerable<T> source, Func<Task<R>> empty, Func<T, IAsyncEnumerable<T>, Task<R>> more)
    {
        var head = await source.Head();
        return await head.MatchAsync(
            Some: x => more(x, source.Skip(1)),
            None: () => empty());
    }
}
