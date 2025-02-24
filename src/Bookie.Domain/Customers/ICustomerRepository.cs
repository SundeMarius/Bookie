using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;

namespace Bookie.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> FindAsync(CustomerQuery query);
    Task<IEnumerable<Loan>> GetLoans(Guid customerId);
}

public record CustomerQuery(
    string? FirstName = null,
    string? LastName = null,
    string? Email = null,
    AuthorizationLevel? MinimumAuthorization = null
);