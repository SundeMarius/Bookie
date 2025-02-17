using Bookie.Domain.Abstractions;

namespace Bookie.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> FindAsync(CustomerQuery query);
}

public record CustomerQuery(string? FirstName = null, string? LastName = null, string? Email = null);