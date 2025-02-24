using Bookie.Domain.Customers;

namespace Bookie.Application.Customers;

public record CustomerDto(Guid Id, string FirstName, string LastName, string Email, List<LoanDto> Rentals, DateTimeOffset Created)
{
    public static CustomerDto ToCustomerDto(Customer customer) => new(
        customer.Id,
        customer.FirstName,
        customer.LastName,
        customer.Email,
        [.. customer.Loans.Select(LoanDto.ToLoanDto)],
        customer.Created
    );
}
