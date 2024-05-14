using System.Text.Json;
using EventStore.Client;
using Domain.Order;
using Domain.Product;
using Domain.Customer;
using Infrastructure;

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

var customerCreated = new CustomerCreated("Pepe Le Put", "Pepe.LePut@comics.com", "123-234-4567");

var customerId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0a"));

var eventData = new EventData(
    customerId,
    nameof(CustomerCreated),
    JsonSerializer.SerializeToUtf8Bytes(customerCreated)
);

await client.AppendToStreamAsync(
    $"customer-{customerId}",
    StreamState.Any,
    [eventData]
);

var productCreated = new ProductCreated("Car Fragant", 100, 100);

var productId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0b"));
;

eventData = new EventData(
    productId,
    nameof(ProductCreated),
    JsonSerializer.SerializeToUtf8Bytes(productCreated)
);

await client.AppendToStreamAsync(
    $"product-{productId}",
    StreamState.Any,
    [eventData]
);

var orderCreated = new OrderCreated(new CustomerId(customerId.ToGuid()), new ProductId(productId.ToGuid()), 100);

var orderId = Uuid.FromGuid(Guid.Parse("f5b1f1e1-0b0b-4b1e-8b3d-3e1f0a3b0f0c"));

eventData = new EventData(
    orderId,
    nameof(OrderCreated),
    JsonSerializer.SerializeToUtf8Bytes(orderCreated)
);

await client.AppendToStreamAsync(
    $"order-{orderId}",
    StreamState.Any,
    [eventData]
);

var customerNameChanged = new CustomerNameChanged("Pepe LePut");

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(CustomerNameChanged),
    JsonSerializer.SerializeToUtf8Bytes(customerNameChanged)
);

await client.AppendToStreamAsync(
    $"customer-{customerId}",
    StreamState.Any,
    [eventData]
);

var productPriceChanged = new ProductPriceChanged(50);

productPriceChanged.GetHashCode();

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(ProductPriceChanged),
    JsonSerializer.SerializeToUtf8Bytes(productPriceChanged)
);

await client.AppendToStreamAsync(
    $"product-{productId}",
    StreamState.Any,
    [eventData]
);

var orderQuantityChanged = new OrderQuantityChanged(50);

eventData = new EventData(
    Uuid.NewUuid(),
    nameof(OrderQuantityChanged),
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