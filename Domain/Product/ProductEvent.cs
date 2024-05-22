using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product;

public abstract record ProductEvent(Guid Id, DateTime Timestamp) : Event(Id, Timestamp);

public record ProductCreatedEvent(Guid Id, DateTime Timestamp, string Name, decimal Price, int Quantity) : ProductEvent(Id, Timestamp);

public record ProductNameChangedEvent(Guid Id, DateTime Timestamp, string Name) : ProductEvent(Id, Timestamp);

public record ProductPriceChangedEvent(Guid Id, DateTime Timestamp, decimal Price) : ProductEvent(Id, Timestamp);

public record ProductQuantityChangedEvent(Guid Id, DateTime Timestamp, int Quantity) : ProductEvent(Id, Timestamp);

public record ProductDiscontinuedEvent(Guid Id, DateTime Timestamp) : ProductEvent(Id, Timestamp);

