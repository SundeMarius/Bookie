using Bookie.Domain.Customers;

namespace Bookie.Infrastructure.Repositories.Memory;

public class CustomerRepository : ICustomerRepository
{
    private readonly Dictionary<Guid, Customer> _customers = [];

    public Task<Customer> CreateAsync(Customer entity)
    {
        _customers.Add(entity.Id, entity);
        return Task.FromResult(entity);
    }

    public async Task<Customer?> DeleteAsync(Guid id)
    {
        if (!_customers.TryGetValue(id, out var customer))
            return await Task.FromResult<Customer?>(null);
        _customers.Remove(id);
        return customer;
    }

    public async Task<IEnumerable<Customer>> FindAsync(CustomerQuery query)
    {
        var it = _customers.Values.AsEnumerable();
        if (query.FirstName is not null)
            it = it.Where(c => c.FirstName.Contains(query.FirstName, StringComparison.CurrentCultureIgnoreCase));
        if (query.LastName is not null)
            it = it.Where(c => c.LastName.Contains(query.LastName, StringComparison.CurrentCultureIgnoreCase));
        if (query.Email is not null)
            it = it.Where(c => c.Email is not null && c.Email.Contains(query.Email, StringComparison.CurrentCultureIgnoreCase));
        if (query.MinimumAuthorization is not null)
            it = it.Where(c => c.Authorization >= query.MinimumAuthorization);

        return await Task.FromResult(it);
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        if (!_customers.TryGetValue(id, out var customer))
            return await Task.FromResult<Customer?>(null);
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer entity)
    {
        _customers[entity.Id] = entity;
        return await Task.FromResult(entity);
    }

    public async Task<IEnumerable<Loan>> GetLoans(Guid CustomerId)
    {
        var customer = _customers.Values.FirstOrDefault(c => c.Id == CustomerId);
        if (customer is null)
            return await Task.FromResult(new List<Loan>());
        return customer.Loans;
    }
}
