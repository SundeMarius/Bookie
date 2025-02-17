namespace Bookie.Application.Customers;

public record CustomerDto(string FirstName, string? LastName, string? Email, List<RentalDto> Rentals);
