using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain.Customer;

public static class Customer
{
    public static CustomerState Create(CustomerCreated @event)
        => new(new CustomerId(Guid.NewGuid()), @event.Name, @event.Email, @event.Phone);

    public static CustomerState Apply(this CustomerState state, CustomerEvent @event)
        => @event switch
        {
            CustomerNameChanged e => state with { Name = e.Name },
            CustomerEmailChanged e => state with { Email = e.Email },
            CustomerPhoneChanged e => state with { Phone = e.Phone },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };

    public async static Task<Option<CustomerState>> From(IAsyncEnumerable<CustomerEvent> history)
        => await history.MatchAsync<CustomerEvent, Option<CustomerState>>(
            empty: () => Task.FromResult(Option<CustomerState>.None),
            more: (created, otherEvents)
                => Optional(otherEvents.AggregateAsync(
                        Customer.Create((CustomerCreated)created),
                        (state, @event) => state.Apply(@event))
                    .AsTask())
                    .Sequence()
            );
}

public static class IAsyncEnumerableExtensions
{
    public static async Task<Option<T>> Head<T>(this IAsyncEnumerable<T> source)
    {
        var enumerator = source.GetAsyncEnumerator();
        bool isNext = await enumerator.MoveNextAsync();
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
