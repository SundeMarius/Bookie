using Bookie.Domain.Customers;

namespace Bookie.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> Find(CustomerQuery query);
    Task<Customer?> Create(Customer customer);
    Task<Customer?> Get(Guid customerId);
    Task<Customer?> Update(Customer customer);
    Task<Customer?> Delete(Guid customerId);
}

public record CustomerQuery(string? FirstName = null, string? LastName = null, string? Email = null);