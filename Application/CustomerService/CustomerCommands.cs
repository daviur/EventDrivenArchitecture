using Domain.Customer;

namespace Application.CustomerService;

public record CreateCustomerCommand(string Name, string Email, string Phone, DateTime Timestamp) : Command(Timestamp)
{
    public CustomerCreatedEvent ToEvent() => new CustomerCreatedEvent(Guid.NewGuid(), DateTime.UtcNow, Name, Email, Phone);
}

public record ChangeCustomerNameCommand(string Name, DateTime Timestamp) : Command(Timestamp)
{
    public CustomerNameChangedEvent ToEvent() => new CustomerNameChangedEvent(Guid.NewGuid(), DateTime.UtcNow, Name);
}

public record ChangeCustomerEmailCommand(string Email, DateTime Timestamp) : Command(Timestamp)
{
    public CustomerEmailChangedEvent ToEvent() => new CustomerEmailChangedEvent(Guid.NewGuid(), DateTime.UtcNow, Email);
}

public record ChangeCustomerPhoneCommand(string Phone, DateTime Timestamp) : Command(Timestamp)
{
    public CustomerPhoneChangedEvent ToEvent() => new CustomerPhoneChangedEvent(Guid.NewGuid(), DateTime.UtcNow, Phone);
}