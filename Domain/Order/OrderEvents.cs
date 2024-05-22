using Domain.Customer;
using Domain.Product;

namespace Domain.Order;

public abstract record OrderEvent(Guid Id, DateTime Timestamp) : Event(Id, Timestamp);

public record OrderCreatedEvent(Guid Id, DateTime Timestamp, CustomerId CustomerId, ProductId ProductId, int Quantity) : OrderEvent(Id, Timestamp);

public record OrderSubmittedEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);

public record OrderDeliveredEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);

public record OrderAcceptedEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);

public record OrderRejectedEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);

public record OrderCanceledEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);

public record OrderQuantityChangedEvent(Guid Id, DateTime Timestamp, int Quantity) : OrderEvent(Id, Timestamp);

public record OrderProductChangedEvent(Guid Id, DateTime Timestamp, ProductId ProductId) : OrderEvent(Id, Timestamp);

public record OrderShippedEvent(Guid Id, DateTime Timestamp) : OrderEvent(Id, Timestamp);
