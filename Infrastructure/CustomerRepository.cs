using Domain.Customer;
using EventStore.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure;

public class CustomerRepository : StateRepository<CustomerState, CustomerEvent, CustomerId>
{
    public CustomerRepository(EventStoreClient client) :
        base(client, Customer.Create, Customer.Apply)
    {
    }

    protected override string GetStreamName(CustomerId id)
        => $"customer-{id.Value}";

    protected override CustomerEvent ToEvent(ResolvedEvent @event)
        => @event.Event.EventType switch
        {
            nameof(CustomerCreatedEvent) => JsonSerializer.Deserialize<CustomerCreatedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerNameChangedEvent) => JsonSerializer.Deserialize<CustomerNameChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerEmailChangedEvent) => JsonSerializer.Deserialize<CustomerEmailChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(CustomerPhoneChangedEvent) => JsonSerializer.Deserialize<CustomerPhoneChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            _ => throw new InvalidOperationException($"Unknown event type {@event.Event.EventType}")
        };
}