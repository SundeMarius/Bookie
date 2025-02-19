using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;

namespace Bookie.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> FindAsync(CustomerQuery query);
    Task<Rental?> GetRental(Guid RentalId);
    Task<IEnumerable<Rental>> GetRentals(Guid CustomerId);
}

public record CustomerQuery(
    string? FirstName = null,
    string? LastName = null,
    string? Email = null,
    AuthorizationLevel? Authorization = null
);