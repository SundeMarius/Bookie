using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Domain.Customers;

public class Rental : Entity
{
    public static Result<Rental> Create(Guid bookId, DateTimeOffset from, DateTimeOffset to)
    {
        if (from >= to)
            return RentalErrors.InvalidRentalPeriod(from, to);
        if (from.AddMonths(2) <= to)
            return RentalErrors.RentalTooLong;
        return new Rental(bookId, from, to);
    }
    public Guid BookId { get; private set; }
    public DateTimeOffset From { get; private set; }
    public DateTimeOffset To { get; private set; }

    private Rental(Guid bookId, DateTimeOffset from, DateTimeOffset to) : base(Guid.NewGuid())
    {
        BookId = bookId;
        From = from;
        To = to;
    }
}

public static class RentalErrors
{
    public static BookieError InvalidRentalPeriod(DateTimeOffset from, DateTimeOffset to)
        => new("RentalErrors.InvalidRentalPeriod", $"The 'from' date {from} has to be before the 'to' date {to}");

    public static BookieError RentalTooLong
        => new("RentalErrors.RentalTooLong", $"The rental period can be at most two months");
}