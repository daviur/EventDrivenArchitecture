using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product;
public static class Product
{
    public static ProductState Create(ProductCreated @event)
        => new(new ProductId(Guid.NewGuid()), @event.Name, @event.Price, @event.Quantity, ProductStatus.Active);

    public static ProductState Apply(this ProductState state, ProductEvent @event)
        => @event switch
        {
            ProductNameChanged e => state with { Name = e.Name },
            ProductPriceChanged e => state with { Price = e.Price },
            ProductQuantityChanged e => state with { Quantity = e.Quantity },
            ProductDiscontinued _ => state with { Status = ProductStatus.Discontinued },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };
}
