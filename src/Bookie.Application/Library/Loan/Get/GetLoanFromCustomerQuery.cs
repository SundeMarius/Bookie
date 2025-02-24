using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Domain.Customers;

namespace Bookie.Application.Library.Rental.Get;

public class GetRentalsFromCustomerQueryHandler(ICustomerRepository customerRepository) : IQueryHandler<GetLoansForCustomerQuery, List<LoanDto>>
{
    public async Task<List<LoanDto>> Handle(GetLoansForCustomerQuery request, CancellationToken cancellationToken)
    {
        return [.. (await customerRepository.GetLoans(request.CustomerId)).Select(LoanDto.ToLoanDto)];
    }
}

public record GetLoansForCustomerQuery(Guid CustomerId) : IQuery<List<LoanDto>>;
