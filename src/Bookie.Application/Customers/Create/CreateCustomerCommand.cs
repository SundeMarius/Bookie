using Bookie.Application.Abstractions;
using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using FluentMonads;
using FluentValidation;

namespace Bookie.Application.Customers.Create;

public class CreateCustomerCommandHandler(LibraryService library) : ICommandHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validationResult = new CreateCustomerCommandValidator().Validate(command);
        if (!validationResult.IsValid)
            return new ValidationError(validationResult.ToString());

        return (await Customer
                .Create(command.FirstName, command.LastName, command.Email, command.Authorization)
                .AndThenAsync(library.RegisterCustomer))
                .Map(CustomerDto.ToCustomerDto);

    }
}

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    AuthorizationLevel Authorization) : ICommand<CustomerDto>;

class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(createBookCommand => createBookCommand.FirstName).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.LastName).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.Email).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.Authorization).IsInEnum();
    }
}