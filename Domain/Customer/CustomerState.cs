namespace Domain.Customer;

public readonly record struct CustomerId(Guid Value);

public record CustomerState(CustomerId Id, string Name, string Email, string Phone);