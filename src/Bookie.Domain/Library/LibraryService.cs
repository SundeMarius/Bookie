using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Repositories;
using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Domain.Library;

public class LibraryService(IBookRepository bookRepository, ICustomerRepository customerRepository)
{
    public async Task<Result<Customer>> RentBook(Guid customerId, Guid bookId, DateTimeOffset from, DateTimeOffset to)
    {
        var customer = await customerRepository.Get(customerId);
        if (customer is null)
            return LibraryErrors.CustomerDoesNotExist(customerId);

        var bookInventory = await bookRepository.Get(bookId);
        if (bookInventory is null)
            return LibraryErrors.BookDoesNotExist(bookId);

        if (bookInventory.InventoryCount == 0)
            return LibraryErrors.BookNotAvailable(bookInventory.Book.ISBN10);

        return await Rental.Create(bookId, from, to)
                .OnSuccess(async rental =>
                {
                    await bookRepository.UpdateBookCount(rental.BookId, bookInventory.InventoryCount - 1);
                })
                .AndThenAsync<Rental, Customer>(async rental =>
                {
                    customer.AddRental(rental);
                    await customerRepository.Update(customer);
                    return customer;
                });
    }

    public async Task<Result<Customer>> EndBookRental(Guid customerId, Guid bookId)
    {
        var customer = await customerRepository.Get(customerId);
        if (customer is null)
            return LibraryErrors.CustomerDoesNotExist(customerId);

        var bookInventory = await bookRepository.Get(bookId);
        if (bookInventory is null)
            return LibraryErrors.BookDoesNotExist(bookId);

        customer.RemoveRental(bookId);
        await customerRepository.Update(customer);
        return customer;
    }

    public Task<IEnumerable<BookQueryResult>> FindBooks(BookQuery bookQuery)
    {
        return bookRepository.Find(bookQuery);
    }

    public async Task<Result<Book>> AddBook(string bookTitle, string author, DateOnly release, uint group, uint publisher, uint title)
    {
        return await ISBN10
            .Create(group, publisher, title)
            .AndThen(isbn10 => Book.Create(bookTitle, author, release, isbn10))
            .AndThenAsync<Book, Book>(async book =>
            {
                var existingBooks = await FindBooks(new BookQuery(Group: group, Publisher: publisher, Title: title));
                if (existingBooks.Any())
                    return LibraryErrors.BookAlreadyExists(book.ISBN10);
                return book;
            });
    }

    public async Task<Result<Book>> DeleteBook(Guid bookId)
    {
        var book = await bookRepository.Delete(bookId);
        if (book is null)
            return LibraryErrors.BookDoesNotExist(bookId);
        return book;
    }
}

public static class LibraryErrors
{
    public static DomainError BookAlreadyExists(ISBN10 iSBN10) => new("LibraryError.BookAlreadyExists", $"The book with isbn '{iSBN10}' already exists");
    public static DomainError BookDoesNotExist(Guid bookId) => new("LibraryError.BookDoesNotExist", $"The book with id '{bookId}' does not exist");
    public static DomainError BookNotAvailable(ISBN10 iSBN10) => new("LibraryError.BookNotAvailable", $"The book with isbn '{iSBN10}' is not available");

    public static DomainError CustomerDoesNotExist(Guid customerId) => new("LibraryError.CustomerDoesNotExist", $"A customer with id '{customerId}' does not exist");
}