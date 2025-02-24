using Bookie.Domain.Customers;

namespace Bookie.Application.Customers;

public record LoanDto(Guid BookId, DateOnly From, DateOnly To)
{
    public static LoanDto ToLoanDto(Loan loan) => new(
        loan.BookId,
        loan.Period.From,
        loan.Period.To
    );
}
