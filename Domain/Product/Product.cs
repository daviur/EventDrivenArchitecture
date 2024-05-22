namespace Domain.Product;

public static class Product
{
    public static ProductState Apply(ProductState state, ProductEvent @event)
        => @event switch
        {
            ProductNameChangedEvent e => state with { Name = e.Name },
            ProductPriceChangedEvent e => state with { Price = e.Price },
            ProductQuantityChangedEvent e => state with { Quantity = e.Quantity },
            ProductDiscontinuedEvent _ => state with { Status = ProductStatus.Discontinued },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };
    public static ProductState Create(ProductEvent @event)
    {
        var createEvent = (ProductCreatedEvent)@event;
        return new(new ProductId(Guid.NewGuid()), createEvent.Name, createEvent.Price, createEvent.Quantity, ProductStatus.Active);
    }
}