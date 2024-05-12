using Domain.Customer;
using Domain.Product;

namespace Domain.Order;

public abstract record OrderEvent;

public record OrderCreated(CustomerId CustomerId, ProductId ProductId, int Quantity) : OrderEvent;

public record OrderSubmitted : OrderEvent;

public record OrderDelivered : OrderEvent;

public record OrderAccepted : OrderEvent;

public record OrderRejected : OrderEvent;

public record OrderCanceled : OrderEvent;

public record OrderQuantityChanged(int Quantity) : OrderEvent;

public record OrderProductChanged(ProductId ProductId) : OrderEvent;

public record OrderShipped : OrderEvent;
