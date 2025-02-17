using Bookie.Domain.Abstractions;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using FluentMonads;

namespace Bookie.Domain.Library;

public class LibraryService(IBookRepository bookRepository, ICustomerRepository customerRepository)
{
    public Result RentBook(Customer customer, BookRecord bookRecord, DateTimeOffset from, DateTimeOffset to)
    {
        if (bookRecord.InventoryCount == 0)
            return Result.Failure(LibraryErrors.BookNotAvailable(bookRecord.Book.ISBN10));

        if (customer.Authorization < bookRecord.Book.MinimumAuthorization)
            return Result.Failure(LibraryErrors.NotAuthorized(customer, bookRecord.Book));

        return (Result)Rental
                .Create(bookRecord.Book.Id, from, to)
                .OnSuccess(async rental =>
                {
                    await bookRepository.UpdateBookCountAsync(rental.BookId, bookRecord.InventoryCount - 1);
                    customer.AddRental(rental);
                    await customerRepository.UpdateAsync(customer);
                })
                .AndThen(_ => Result.Success());
    }

    public async Task<Result> EndBookRental(Customer customer, Book book)
    {
        if (!customer.Rentals.Any(r => r.Id == book.Id))
            return Result.Failure(LibraryErrors.BookNotRented(customer.Id, book.ISBN10));

        customer.RemoveRental(book.Id);

        await customerRepository.UpdateAsync(customer);
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

    public async Task<Result<Book>> DeleteBook(Guid id)
    {
        var deletedBook = await bookRepository.DeleteAsync(id);
        if (deletedBook is null)
            return LibraryErrors.BookNotFound(id);
        return deletedBook;
    }
}

public static class LibraryErrors
{
    public static DomainError BookNotRented(Guid customerId, ISBN10 iSBN10)
        => new("LibraryError.BookNotRented", $"No rental of isbn '{iSBN10}' registered on customer with id '{customerId}'");
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