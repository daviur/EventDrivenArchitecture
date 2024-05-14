using Domain.Customer;
using EventStore.Client;
using LanguageExt;
using LanguageExt.Common;
using System.Text;
using System.Text.Json;
using static LanguageExt.Prelude;

namespace Infrastructure;

public class CustomerRepository
{
    private readonly EventStoreClient _eventStoreClient;

    public CustomerRepository(EventStoreClient eventStoreClient)
    {
        _eventStoreClient = eventStoreClient;
    }

    public async Task<Option<CustomerState>> Get(CustomerId id)
    {
        var streamName = $"customer-{id.Value}";
        var events = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);
        if (await events.ReadState == ReadState.StreamNotFound) return None;
        var history = events.AsAsyncEnumerable().Select(ToEvent);
        return await Customer.From(history);
    }

    private CustomerEvent ToEvent(ResolvedEvent @event)
        => @event.Event.EventType switch
        {
            nameof(CustomerCreated) => JsonSerializer.Deserialize<CustomerCreated>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerNameChanged) => JsonSerializer.Deserialize<CustomerNameChanged>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerEmailChanged) => JsonSerializer.Deserialize<CustomerEmailChanged>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerPhoneChanged) => JsonSerializer.Deserialize<CustomerPhoneChanged>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            _ => throw new InvalidOperationException($"Unknown event type {@event.Event.EventType}")
        };

    public async Task Save(CustomerId id, IEnumerable<CustomerEvent> events)
    {
        var streamName = $"customer-{id.Value}";
        var eventData = events.Select(e => new EventData(Uuid.NewUuid(), e.GetType().Name, JsonSerializer.SerializeToUtf8Bytes(e)));

        await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any, eventData);
    }
}