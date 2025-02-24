using Bookie.Domain.Abstractions;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using FluentMonads;

namespace Bookie.Domain.Library;

public class LibraryService(IBookRepository bookRepository, ICustomerRepository customerRepository)
{
    public async Task<Result<Loan>> LoanBook(Customer customer, BookRecord bookRecord, Period loanPeriod)
    {
        if (bookRecord.InventoryCount == 0)
            return LibraryError.BookNotAvailable(bookRecord.Book.ISBN10);

        if (customer.Authorization < bookRecord.Book.MinimumAuthorization)
            return LibraryError.NotAuthorized(customer, bookRecord.Book);

        return await Loan
                .Create(bookRecord.Book.Id, loanPeriod)
                .OnSuccessAsync(async rental =>
                {
                    await bookRepository.DecrementBookCountAsync(rental.BookId);
                    customer.AddLoan(rental);

                    await customerRepository.UpdateAsync(customer);
                });
    }

    public async Task<Result> ReturnBook(Customer customer, Loan loan)
    {
        if (!customer.Loans.Any(r => r == loan))
            return Result.Failure(LibraryError.BookNotBorrowed(customer.Id, loan.BookId));

        customer.RemoveLoan(loan);
        await bookRepository.IncrementBookCountAsync(loan.BookId);

        await customerRepository.UpdateAsync(customer);
        return Result.Success();
    }

    public async Task<Result<Book>> AddBook(Book book)
    {
        var existingBooks = await bookRepository.FindAsync(
            group: book.ISBN10.Group,
            publisher: book.ISBN10.Publisher,
            title: book.ISBN10.Title);
        if (existingBooks.Any())
            return LibraryError.BookAlreadyAdded(book.ISBN10);

        return await bookRepository.CreateAsync(book);
    }

    public async Task<Result> DeleteBook(Guid bookId)
    {
        var deletedBook = await bookRepository.DeleteAsync(bookId);
        if (deletedBook is null)
            return Result.Failure(LibraryError.BookNotFound(bookId));
        return Result.Success();
    }

    public async Task<Result<Customer>> RegisterCustomer(Customer customer)
    {
        if (customer.Email is not null)
        {
            var existingCustomer = await customerRepository.FindAsync(new(Email: customer.Email));
            if (existingCustomer.Any())
                return LibraryError.CustomerWithMatchingEmail(customer.Email);
        }

        return await customerRepository.CreateAsync(customer);
    }

    public async Task<Result> UnregisterCustomer(Guid customerId)
    {
        var deletedCustomer = await customerRepository.DeleteAsync(customerId);
        if (deletedCustomer is null)
            return Result.Failure(LibraryError.CustomerNotFound(customerId));
        return Result.Success();
    }
}

public static class LibraryError
{
    public static DomainError BookNotBorrowed(Guid customerId, Guid bookId)
        => new($"LibraryError.BookNotBorrowed", $"Customer with id '{customerId}' is not borrowing book with id '{bookId}'");
    public static DomainError BookNotAvailable(ISBN10 iSBN10)
        => new("LibraryError.BookNotAvailable", $"The book with isbn '{iSBN10}' is not available");
    public static DomainError BookAlreadyAdded(ISBN10 iSBN10)
        => new("LibraryError.BookAlreadyAdded", $"The book with isbn '{iSBN10}' already added");
    public static DomainError BookNotFound(Guid bookId)
        => new("LibraryError.BookNotFound", $"The book with id '{bookId}' does not exist");
    public static DomainError NotAuthorized(Customer customer, Book book)
        => new("LibraryError.Unauthorized",
            $"The customer has access level '{customer.Authorization}' but isbn'{book.ISBN10}' requires at least access level '{book.MinimumAuthorization}'");
    public static DomainError CustomerNotFound(Guid id)
        => new("LibraryError.CustomerNotFound", $"Customer with id '{id}' not found");
    public static DomainError CustomerWithMatchingEmail(string email)
        => new("LibraryError.DuplicateCustomerEmail", $"Customer with email '{email}' already registered");
}