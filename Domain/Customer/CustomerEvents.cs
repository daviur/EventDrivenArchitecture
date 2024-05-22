using System.Numerics;

namespace Domain.Customer;

public abstract record CustomerEvent(Guid Id, DateTime Timestamp) : Event(Id, Timestamp);

public record CustomerCreatedEvent(Guid Id, DateTime Timestamp, string Name, string Email, string Phone) : CustomerEvent(Id, Timestamp);

public record CustomerNameChangedEvent(Guid Id, DateTime Timestamp, string Name) : CustomerEvent(Id, Timestamp);

public record CustomerEmailChangedEvent(Guid Id, DateTime Timestamp, string Email) : CustomerEvent(Id, Timestamp);

public record CustomerPhoneChangedEvent(Guid Id, DateTime Timestamp, string Phone) : CustomerEvent(Id, Timestamp);
