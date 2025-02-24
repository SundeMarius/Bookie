using Bookie.Application.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Customers;

namespace Bookie.Application.Customers.Get;

public class FindCustomerQueryHandler(ICustomerRepository customerRepository) : IQueryHandler<FindCustomerQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(FindCustomerQuery request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.FindAsync(
            new(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Authorization
                )
            );

        return customers.Select(CustomerDto.ToCustomerDto);
    }
}

public record FindCustomerQuery(
    string? FirstName,
    string? LastName,
    string? Email,
    AuthorizationLevel? Authorization
) : IQuery<IEnumerable<CustomerDto>>;
