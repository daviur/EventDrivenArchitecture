using System.Text.Json;
using EventStore.Client;
using Domain.Order;
using Domain.Product;
using Domain.Customer;
using Infrastructure;

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);


var customerId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0a"));
var customerCreated = new CustomerCreatedEvent(customerId.ToGuid(), DateTime.UtcNow, "Pepe Le Put", "Pepe.LePut@comics.com", "123-234-4567");

var eventData = new EventData(
    customerId,
    nameof(CustomerCreatedEvent),
    JsonSerializer.SerializeToUtf8Bytes(customerCreated)
);

await client.AppendToStreamAsync(
    $"customer-{customerId}",
    StreamState.Any,
    [eventData]
);

var productId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0b"));
var productCreated = new ProductCreatedEvent(productId.ToGuid(), DateTime.UtcNow, "Car Fragant", 100, 100);

eventData = new EventData(
    productId,
    nameof(ProductCreatedEvent),
    JsonSerializer.SerializeToUtf8Bytes(productCreated)
);

await client.AppendToStreamAsync(
    $"product-{productId}",
    StreamState.Any,
    [eventData]
);

var orderId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0c"));
var orderCreated = new OrderCreatedEvent(orderId.ToGuid(), DateTime.UtcNow, new CustomerId(customerId.ToGuid()), new ProductId(productId.ToGuid()), 100);

eventData = new EventData(
    orderId,
    nameof(OrderCreatedEvent),
    JsonSerializer.SerializeToUtf8Bytes(orderCreated)
);

await client.AppendToStreamAsync(
    $"order-{orderId}",
    StreamState.Any,
    [eventData]
);

var customerNameChanged = new CustomerNameChangedEvent(customerId.ToGuid(), DateTime.UtcNow, "Pepe LePut");

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(CustomerNameChangedEvent),
    JsonSerializer.SerializeToUtf8Bytes(customerNameChanged)
);

await client.AppendToStreamAsync(
    $"customer-{customerId}",
    StreamState.Any,
    [eventData]
);

var productPriceChanged = new ProductPriceChangedEvent(productId.ToGuid(), DateTime.UtcNow, 50);

productPriceChanged.GetHashCode();

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(ProductPriceChangedEvent),
    JsonSerializer.SerializeToUtf8Bytes(productPriceChanged)
);

await client.AppendToStreamAsync(
    $"product-{productId}",
    StreamState.Any,
    [eventData]
);

var orderQuantityChanged = new OrderQuantityChangedEvent(orderId.ToGuid(), DateTime.UtcNow, 50);

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(OrderQuantityChangedEvent),
    JsonSerializer.SerializeToUtf8Bytes(orderQuantityChanged)
);

await client.AppendToStreamAsync(
    $"order-{orderId}",
    StreamState.Any,
    [eventData]
);

var customerRepository = new CustomerRepository(client);

var customer = await customerRepository.Get(new CustomerId(customerId.ToGuid()));

Console.WriteLine(customer.Match(
       Some: c => $"{c}",
          None: "Customer not found"
          ));