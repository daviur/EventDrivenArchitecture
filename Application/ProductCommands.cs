namespace Application;

public record CreateProductCommand(string Name, decimal Price, int Quantity, DateTime Timestamp) : Command(Timestamp);

public record ChangeProductNameCommand(string Name, DateTime Timestamp) : Command(Timestamp);

public record ChangeProductPriceCommand(decimal Price, DateTime Timestamp) : Command(Timestamp);

public record ChangeProductQuantityCommand(int Quantity, DateTime Timestamp) : Command(Timestamp);
