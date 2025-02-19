using Bookie.Application.Abstractions;
using Bookie.Domain.Customers;

namespace Bookie.Application.Customers.Get;

public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository) : IQueryHandler<GetCustomerByIdQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(query.Id);
        if (customer is null)
            return null;
        return CustomerDto.ToCustomerDto(customer);
    }
}

public record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDto?>;
