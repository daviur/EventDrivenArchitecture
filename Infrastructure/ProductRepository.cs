using Domain.Product;
using EventStore.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure;

public class ProductRepository : StateRepository<ProductState, ProductEvent, ProductId>
{

    public ProductRepository(EventStoreClient eventStoreClient) : 
        base(eventStoreClient, Product.Create, Product.Apply)
    {
    }

    protected override string GetStreamName(ProductId id)
        => $"product-{id.Value}";

    protected override ProductEvent ToEvent(ResolvedEvent @event)
        => @event.Event.EventType switch
        {
            nameof(ProductCreatedEvent) => JsonSerializer.Deserialize<ProductCreatedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(ProductNameChangedEvent) => JsonSerializer.Deserialize<ProductNameChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(ProductPriceChangedEvent) => JsonSerializer.Deserialize<ProductPriceChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(ProductQuantityChangedEvent) => JsonSerializer.Deserialize<ProductQuantityChangedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            nameof(ProductDiscontinuedEvent) => JsonSerializer.Deserialize<ProductDiscontinuedEvent>(Encoding.UTF8.GetString(@event.Event.Data.ToArray()))!,
            _ => throw new InvalidOperationException($"Unknown event {@event.Event.EventType}")
        };
}
