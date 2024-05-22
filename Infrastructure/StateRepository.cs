using Domain;
using Domain.Customer;
using EventStore.Client;
using LanguageExt;
using System.Text.Json;
using static LanguageExt.Prelude;

namespace Infrastructure;

public abstract class StateRepository<ST, ET, IT> where IT : IHasEntityId
{
    private readonly EventStoreClient _eventStoreClient;
    private readonly Func<ET, ST> creator;
    private readonly Func<ST, ET, ST> aggregator;

    protected StateRepository(EventStoreClient eventStoreClient, Func<ET, ST> creator, Func<ST, ET, ST> aggregator)
    {
        _eventStoreClient = eventStoreClient;
        this.creator = creator;
        this.aggregator = aggregator;
    }

    public async Task<Option<ST>> Get(IT id)
    {
        var streamName = GetStreamName(id);
        var events = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);
        if (await events.ReadState == ReadState.StreamNotFound) return None;
        var history = events.AsAsyncEnumerable().Select(ToEvent);
        return await history.StateCalculator<ST, ET>().Invoke(creator, aggregator);

    }

    public async Task<Unit> Save(IT id, params CustomerEvent[] events)
    {
        var streamName = GetStreamName(id);
        var eventData = events.Select(@event =>
                   new EventData(Uuid.FromGuid(id.Value), @event.GetType().Name, JsonSerializer.SerializeToUtf8Bytes(@event)));
        await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any, eventData);
        return unit;
    }

    protected abstract string GetStreamName(IT id);

    protected abstract ET ToEvent(ResolvedEvent @event);
}
