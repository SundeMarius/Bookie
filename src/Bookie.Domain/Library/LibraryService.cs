using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Repositories;
using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Domain.Library;

public class LibraryService(IBookRepository bookRepository, ICustomerRepository customerRepository)
{
    public async Task<Result<Customer>> RentBook(Customer customer, BookInventory bookInventory, DateTimeOffset from, DateTimeOffset to)
    {
        if (bookInventory.InventoryCount == 0)
            return LibraryErrors.NotAvailable(bookInventory.Book.ISBN10);

        return await Rental.Create(bookInventory.Book.Id, from, to)
                .OnSuccess(async rental =>
                {
                    await bookRepository.UpdateBookCountAsync(rental.BookId, bookInventory.InventoryCount - 1);
                })
                .AndThenAsync<Rental, Customer>(async rental =>
                {
                    customer.AddRental(rental);
                    await customerRepository.Update(customer);
                    return customer;
                });
    }

    public async Task<Result<Customer>> EndBookRental(Customer customer, Book book)
    {
        if (!customer.Rentals.Any(r => r.Id == book.Id))
            return LibraryErrors.NotRented(customer.Id, book.ISBN10);
        customer.RemoveRental(book.Id);
        await customerRepository.Update(customer);
        return customer;
    }
}

public static class LibraryErrors
{
    public static BookieError NotRented(Guid customerId, ISBN10 iSBN10) => new("LibraryError.NotRented", $"Customer with id '{customerId}' does not rent isbn '{iSBN10}'");
    public static BookieError NotAvailable(ISBN10 iSBN10) => new("LibraryError.NotAvailable", $"The book with isbn '{iSBN10}' is not available");
}