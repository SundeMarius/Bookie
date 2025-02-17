namespace Bookie.Application.Customers;

public record RentalDto(Guid BookId, DateTimeOffset From, DateTimeOffset To);
