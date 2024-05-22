using Domain.Order;
using EventStore.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure;

public class OrderRepository : StateRepository<OrderState, OrderEvent, OrderId>
{

    public OrderRepository(EventStoreClient client) : 
        base(client, Order.Create, Order.Apply)
    {
    }

    protected override string GetStreamName(OrderId id)
        => $"order-{id.Value}";

    protected override OrderEvent ToEvent(ResolvedEvent @event)
        => @event.Event.EventType switch
        {
            nameof(OrderCreatedEvent) => JsonSerializer.Deserialize<OrderCreatedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderSubmittedEvent) => JsonSerializer.Deserialize<OrderSubmittedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderAcceptedEvent) => JsonSerializer.Deserialize<OrderAcceptedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderRejectedEvent) => JsonSerializer.Deserialize<OrderRejectedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderShippedEvent) => JsonSerializer.Deserialize<OrderShippedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderDeliveredEvent) => JsonSerializer.Deserialize<OrderDeliveredEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderCanceledEvent) => JsonSerializer.Deserialize<OrderCanceledEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderQuantityChangedEvent) => JsonSerializer.Deserialize<OrderQuantityChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(OrderProductChangedEvent) => JsonSerializer.Deserialize<OrderProductChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            _ => throw new InvalidOperationException($"Unknown event type {@event.Event.EventType}")
        };
}