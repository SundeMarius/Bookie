using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using FluentMonads;
using FluentValidation;

namespace Bookie.Application.Library.Loan.Create;

public class CreateLoanCommandHandler(LibraryService library, ICustomerRepository customerRepository, IBookRepository bookRepository)
    : ICommandHandler<CreateLoanCommand, LoanDto>
{
    public async Task<Result<LoanDto>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = new CreateLoanCommandValidator().Validate(request);
        if (!validationResult.IsValid)
            return new ValidationError(validationResult.ToString());

        var customer = await customerRepository.GetAsync(request.CustomerId);
        if (customer is null)
            return LibraryError.CustomerNotFound(request.CustomerId);

        var bookRecord = await bookRepository.GetBookRecordAsync(request.BookId);
        if (bookRecord is null)
            return LibraryError.BookNotFound(request.BookId);

        return (await library
                .LoanBook(customer, bookRecord, new Period(request.From, request.To)))
                .Map(LoanDto.ToLoanDto);
    }
}

public record CreateLoanCommand(Guid CustomerId, Guid BookId, DateOnly From, DateOnly To) : ICommand<LoanDto>;

class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.BookId).NotEmpty();
        RuleFor(command => command.From).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(command => command.To).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }
}
