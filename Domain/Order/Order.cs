using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain.Order;

public static class Order
{
    public static OrderState Create(OrderCreated @event)
        => new(new OrderId(Guid.NewGuid()), @event.CustomerId, @event.ProductId, @event.Quantity, OrderStatus.Submitted);

    public static OrderState Apply(this OrderState state, OrderEvent @event)
        => @event switch
        {
            OrderSubmitted => state with { Status = OrderStatus.Submitted },
            OrderAccepted => state with { Status = OrderStatus.Accepted },
            OrderRejected => state with { Status = OrderStatus.Rejected },
            OrderShipped => state with { Status = OrderStatus.Shipped },
            OrderDelivered => state with { Status = OrderStatus.Delivered },
            OrderCanceled => state with { Status = OrderStatus.Canceled },
            OrderQuantityChanged e => state with { Quantity = e.Quantity },
            OrderProductChanged e => state with { ProductId = e.ProductId },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };

    public static Option<OrderState> From(IEnumerable<OrderEvent> history)
        => history.Match(
            Empty: () => None,
            More: (created, otherEvents) =>
                Optional(otherEvents.Aggregate(
                    Create((OrderCreated)created),
                    (state, @event) => state.Apply(@event)))
        );
}
