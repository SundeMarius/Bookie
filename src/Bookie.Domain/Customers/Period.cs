namespace Bookie.Domain.Customers;

public record Period(DateOnly From, DateOnly To)
{
    public bool IsBackwards => To < From;

    public TimeSpan Duration => TimeSpan.FromDays(To.DayNumber - From.DayNumber);

    public override string ToString() => $"{From} - {To}";
}
