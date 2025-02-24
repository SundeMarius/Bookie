using Bookie.Domain.Abstractions;
using FluentMonads;

namespace Bookie.Domain.Customers;

public class Loan : Entity
{
    public static Result<Loan> Create(Guid bookId, Period loanPeriod)
    {
        if (loanPeriod.IsBackwards)
            return LoanError.InvalidLoanPeriod(loanPeriod);

        if (loanPeriod.Duration > TimeSpan.FromDays(60))
            return LoanError.LoanTooLong;

        return new Loan(bookId, loanPeriod);
    }
    public Guid BookId { get; private set; }
    public Period Period { get; private set; }

    private Loan(Guid bookId, Period period)
    {
        BookId = bookId;
        Period = period;
    }
}

public static class LoanError
{
    public static DomainError InvalidLoanPeriod(Period period)
        => new("LoanError.InvalidLoanPeriod", $"The loan period ({period}) is backwards. Please specify a proper loan period (forward in time)");
    public static DomainError LoanTooLong
        => new("LoanError.TooLong", $"The loan period can at most be two months");
}