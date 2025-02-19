using Bookie.Application.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Customers.Create;

public class CreateCustomerCommandHandler(LibraryService library) : ICommandHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        return (await Customer
                .Create(command.FirstName, command.LastName, command.Email, command.Authorization)
                .AndThenAsync(library.RegisterCustomer))
                .Map(CustomerDto.ToCustomerDto);

    }
}

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string? Email,
    AuthorizationLevel Authorization
) : ICommand<CustomerDto>;
