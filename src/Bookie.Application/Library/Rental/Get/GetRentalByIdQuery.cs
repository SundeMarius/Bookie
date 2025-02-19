using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Domain.Customers;

namespace Bookie.Application.Library.Rental.Get;

public class GetRentalByIdQueryHandler(ICustomerRepository customerRepository) : IQueryHandler<GetRentalByIdQuery, RentalDto?>
{
    public async Task<RentalDto?> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        var rental = await customerRepository.GetRental(request.Id);
        if (rental is null)
            return null;
        return RentalDto.ToRentalDto(rental);
    }
}

public record GetRentalByIdQuery(Guid Id) : IQuery<RentalDto?>;
