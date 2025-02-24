using Bookie.Application.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Customers.Update;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository) : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAsync(request.CustomerId);
        if (customer is null)
            return Result.Failure(LibraryError.CustomerNotFound(request.CustomerId));

        return
            customer
            .Update(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                level: request.Level
            ).OnSuccess(async _ =>
            {
                await customerRepository.UpdateAsync(customer);
            });
    }
}

public record UpdateCustomerCommand(
    Guid CustomerId,
    string? FirstName = null,
    string? LastName = null,
    string? Email = null,
    AuthorizationLevel? Level = null) : ICommand;
