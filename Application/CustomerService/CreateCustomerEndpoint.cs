using Domain.Customer;
using EventStore.Client;
using Infrastructure;
using System.ComponentModel;
using static LanguageExt.Prelude;

namespace Application.CustomerService;

public class CustomerService
{
    private readonly CustomerRepository _repository;

    public CustomerService(CustomerRepository repository)
    {
        _repository = repository;
    }

    public CustomerState CreateCustomer(string name, string email, string phone)
    {
        var command = new CreateCustomerCommand(name, email, phone, DateTime.UtcNow);
        var customerCreatedEvent = command.ToEvent();
        var customer = Customer.Create(customerCreatedEvent);
        _ = _repository.Save(customer.Id, customerCreatedEvent);
        return customer;
    }

    public async Task<CustomerState> ChangeCustomerNameAsync(Guid id, string name)
    {
        var command = new ChangeCustomerNameCommand(name, DateTime.UtcNow);
        var customerNameChangedEvent = command.ToEvent();
        var customer = await _repository.Get(new CustomerId(id))
            .IfNoneAsync(() => raise<CustomerState>(new InvalidOperationException("Customer not found.")));
        _ = _repository.Save(customer.Id, customerNameChangedEvent);
        return customer;
    }
}
