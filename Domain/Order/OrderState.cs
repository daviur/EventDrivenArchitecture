using Domain.Customer;
using Domain.Product;

namespace Domain.Order;

public enum OrderStatus
{
    Submitted,
    Accepted,
    Rejected,
    Shipped,
    Delivered,
    Canceled
}

public readonly record struct OrderId(Guid Value);

public record OrderState(OrderId Id, CustomerId CustomerId, ProductId ProductId, int Quantity, OrderStatus Status);
