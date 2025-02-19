using Bookie.Application.Abstractions;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Customers.Delete;

public class DeleteBookCommandHandler(LibraryService library) : ICommandHandler<DeleteCustomerCommand>
{
    public async Task<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        return await library.UnregisterCustomer(command.CustomerId);
    }
}

public record DeleteCustomerCommand(Guid CustomerId) : ICommand;
