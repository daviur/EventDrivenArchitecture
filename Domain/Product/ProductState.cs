namespace Domain.Product;

public enum ProductStatus
{
    Active,
    Discontinued
}

public readonly record struct ProductId(Guid Value) : IHasEntityId;

public record ProductState(ProductId Id, string Name, decimal Price, int Quantity, ProductStatus Status);
