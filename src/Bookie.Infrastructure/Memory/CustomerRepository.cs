using Bookie.Domain.Customers;

namespace Bookie.Infrastructure.Memory;

public class CustomerRepository : ICustomerRepository
{
    public Task<Customer> CreateAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> FindAsync(CustomerQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> UpdateAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task<Rental?> GetRental(Guid RentalId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Rental>> GetRentals(Guid CustomerId)
    {
        throw new NotImplementedException();
    }

}
