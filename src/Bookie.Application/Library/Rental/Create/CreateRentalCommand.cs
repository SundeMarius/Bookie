using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Library.Rental.Create;

public class CreateRentalCommandHandler(LibraryService library, ICustomerRepository customerRepository, IBookRepository bookRepository)
    : ICommandHandler<CreateRentalCommand, RentalDto>
{
    public async Task<Result<RentalDto>> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAsync(request.CustomerId);
        if (customer is null)
            return LibraryErrors.CustomerNotFound(request.CustomerId);

        var bookRecord = await bookRepository.GetBookCountAsync(request.BookId);
        if (bookRecord is null)
            return LibraryErrors.BookNotFound(request.BookId);

        return (await library.RentBook(customer, bookRecord, request.From, request.To))
                .Map(RentalDto.ToRentalDto);
    }
}

public record CreateRentalCommand(Guid CustomerId, Guid BookId, DateTimeOffset From, DateTimeOffset To) : ICommand<RentalDto>;
