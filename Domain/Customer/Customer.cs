using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Customer;

public static class Customer
{
    public static CustomerState Create(CustomerCreated @event)
        => new(new CustomerId(Guid.NewGuid()), @event.Name, @event.Email, @event.Phone);

    public static CustomerState Apply(this CustomerState state, CustomerEvent @event)
        => @event switch
        {
            CustomerNameChanged e => state with { Name = e.Name },
            CustomerEmailChanged e => state with { Email = e.Email },
            CustomerPhoneChanged e => state with { Phone = e.Phone },
            _ => throw new InvalidOperationException($"Unknown event {@event}")
        };
}
