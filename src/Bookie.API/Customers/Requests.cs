namespace Bookie.API.Customers;

public record CreateLoanRequest(Guid BookId, DateOnly From, DateOnly To);