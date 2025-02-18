using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;

namespace Bookie.Domain.Books;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<BookRecord>> FindAsync(BookQuery bookQuery);
    Task<BookRecord?> DecrementBookCountAsync(Guid bookId);
    Task<BookRecord?> IncrementBookCountAsync(Guid bookId);
    Task<BookRecord?> UpdateBookCountAsync(Guid bookId, uint newCount);
    Task<BookRecord?> GetBookCountAsync(Guid bookId);
}

public record BookQuery(string? BookTitle = null,
                        int? Group = null,
                        int? Publisher = null,
                        int? Title = null,
                        AuthorizationLevel? MinimumAuthorization = null,
                        DateOnly? From = null,
                        DateOnly? To = null);

public class BookRecord(Book book, uint inventoryCount)
{
    public Book Book { get; set; } = book;
    public uint InventoryCount { get; set; } = inventoryCount;
}