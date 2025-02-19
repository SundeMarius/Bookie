using Bookie.Domain.Customers;

namespace Bookie.Application.Customers;

public record CustomerDto(string FirstName, string? LastName, string? Email, List<RentalDto> Rentals)
{
    public static CustomerDto ToCustomerDto(Customer customer) => new(
        customer.FirstName,
        customer.LastName,
        customer.Email,
        [.. customer.Rentals.Select(RentalDto.ToRentalDto)]
    );
}
