namespace Application;

public record CreateOrderCommand(Guid CustomerId, Guid ProductId, int Quantity, DateTime Timestamp) : Command(Timestamp);

public record ChangeOrderQuantityCommand(int Quantity, DateTime Timestamp) : Command(Timestamp);

public record ChangeOrderProductCommand(Guid ProductId, DateTime Timestamp) : Command(Timestamp);