namespace Domain.Order;

public static class Order
{
    public static OrderState Apply(OrderState state, OrderEvent @event)
        => @event switch
        {
            OrderSubmittedEvent => state with { Status = OrderStatus.Submitted },
            OrderAcceptedEvent => state with { Status = OrderStatus.Accepted },
            OrderRejectedEvent => state with { Status = OrderStatus.Rejected },
            OrderShippedEvent => state with { Status = OrderStatus.Shipped },
            OrderDeliveredEvent => state with { Status = OrderStatus.Delivered },
            OrderCanceledEvent => state with { Status = OrderStatus.Canceled },
            OrderQuantityChangedEvent e => state with { Quantity = e.Quantity },
            OrderProductChangedEvent e => state with { ProductId = e.ProductId },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };
    public static OrderState Create(OrderEvent @event)
    {
        var createEvent = (OrderCreatedEvent)@event;
        return new(new OrderId(Guid.NewGuid()), createEvent.CustomerId, createEvent.ProductId, createEvent.Quantity, OrderStatus.Submitted);
    }
}