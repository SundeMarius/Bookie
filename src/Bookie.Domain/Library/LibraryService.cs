using Bookie.Domain.Abstractions;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using FluentMonads;

namespace Bookie.Domain.Library;

public class LibraryService(IBookRepository bookRepository, ICustomerRepository customerRepository)
{
    public async Task<Result> RentBook(Customer customer, BookRecord bookRecord, DateTimeOffset from, DateTimeOffset to)
    {
        if (bookRecord.InventoryCount == 0)
            return Result.Failure(LibraryErrors.BookNotAvailable(bookRecord.Book.ISBN10));

        if (customer.Authorization < bookRecord.Book.MinimumAuthorization)
            return Result.Failure(LibraryErrors.NotAuthorized(customer, bookRecord.Book));

        var result = Rental
                        .Create(bookRecord.Book.Id, from, to)
                        .OnSuccess(async rental =>
                        {
                            customer.AddRental(rental);
                            await customerRepository.UpdateAsync(customer);
                            await bookRepository.DecrementBookCountAsync(rental.BookId);
                        });

        return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
    }

    public async Task<Result> ReturnBook(Customer customer, Rental rental)
    {
        if (!customer.Rentals.Any(r => r == rental))
            return Result.Failure(LibraryErrors.BookNotRented(customer.Id, rental.BookId));

        customer.RemoveRental(rental);
        await customerRepository.UpdateAsync(customer);
        await bookRepository.IncrementBookCountAsync(rental.BookId);

        return Result.Success();
    }

    public async Task<Result<Book>> AddBook(Book book)
    {
        var existingBooks = await bookRepository.FindAsync(new(
            Group: book.ISBN10.Group,
            Publisher: book.ISBN10.Publisher,
            Title: book.ISBN10.Title));
        if (existingBooks.Any())
            return LibraryErrors.BookAlreadyAdded(book.ISBN10);

        return await bookRepository.CreateAsync(book);
    }

    public async Task<Result> DeleteBook(Guid id)
    {
        var deletedBook = await bookRepository.DeleteAsync(id);
        if (deletedBook is null)
            return Result.Failure(LibraryErrors.BookNotFound(id));
        return Result.Success();
    }
}

public static class LibraryErrors
{
    public static DomainError BookNotRented(Guid customerId, Guid bookId)
        => new("LibraryError.BookNotRented", $"No rental of book with id '{bookId}' registered on customer with id '{customerId}'");
    public static DomainError BookNotAvailable(ISBN10 iSBN10)
        => new("LibraryError.BookNotAvailable", $"The book with isbn '{iSBN10}' is not available");
    public static DomainError BookAlreadyAdded(ISBN10 iSBN10)
        => new("LibraryError.BookAlreadyAdded", $"The book with isbn '{iSBN10}' already added");
    public static DomainError BookNotFound(Guid bookId)
        => new("LibraryError.BookNotFound", $"The book with id '{bookId}' does not exist");
    public static DomainError NotAuthorized(Customer customer, Book book)
        => new(
            "LibraryError.Unauthorized",
            $"The customer has access level '{customer.Authorization}' but isbn'{book.ISBN10}' requires at least access level '{book.MinimumAuthorization}'");
}