using Bookie.Domain.Customers;

namespace Bookie.Application.Customers;

public record RentalDto(Guid BookId, DateTimeOffset From, DateTimeOffset To)
{
    public static RentalDto ToRentalDto(Rental rental) => new(
        rental.BookId,
        rental.From,
        rental.To
    );
}
