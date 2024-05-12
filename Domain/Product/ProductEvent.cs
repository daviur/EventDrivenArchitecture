using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product;

public abstract record ProductEvent;

public record ProductCreated(string Name, decimal Price, int Quantity) : ProductEvent;

public record ProductNameChanged(string Name) : ProductEvent;

public record ProductPriceChanged(decimal Price) : ProductEvent;

public record ProductQuantityChanged(int Quantity) : ProductEvent;

public record ProductDiscontinued : ProductEvent;

