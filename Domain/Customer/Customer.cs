namespace Domain.Customer;

public static class Customer
{
    public static CustomerState Apply(CustomerState customer, CustomerEvent @event)
        => @event switch
        {
            CustomerNameChangedEvent e => customer with { Name = e.Name },
            CustomerEmailChangedEvent e => customer with { Email = e.Email },
            CustomerPhoneChangedEvent e => customer with { Phone = e.Phone },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };

    public static CustomerState Create(CustomerEvent @event)
    {
        var createEvent = (CustomerCreatedEvent)@event;
        return new(new CustomerId(Guid.NewGuid()), createEvent.Name, createEvent.Email, createEvent.Phone);
    }
}