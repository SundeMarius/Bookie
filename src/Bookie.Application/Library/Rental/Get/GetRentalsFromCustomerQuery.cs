using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Domain.Customers;

namespace Bookie.Application.Library.Rental.Get;

public class GetRentalsFromCustomerQueryHandler(ICustomerRepository customerRepository) : IQueryHandler<GetRentalsFromCustomerQuery, List<RentalDto>>
{
    public async Task<List<RentalDto>> Handle(GetRentalsFromCustomerQuery request, CancellationToken cancellationToken)
    {
        return [.. (await customerRepository.GetRentals(request.CustomerId)).Select(RentalDto.ToRentalDto)];
    }
}

public record GetRentalsFromCustomerQuery(Guid CustomerId) : IQuery<List<RentalDto>>;
