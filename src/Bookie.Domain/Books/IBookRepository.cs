using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;

namespace Bookie.Domain.Books;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<BookRecord>> FindAsync(string? bookTitle = null, string? author = null, int? group = null, int? publisher = null,
                                            int? title = null, AuthorizationLevel? minimumAuthorization = null, DateOnly? releasedFrom = null, DateOnly? releasedto = null);
    Task<BookRecord?> DecrementBookCountAsync(Guid bookId);
    Task<BookRecord?> IncrementBookCountAsync(Guid bookId);
    Task<BookRecord?> UpdateBookCountAsync(Guid bookId, int newCount);
    Task<BookRecord?> GetBookRecordAsync(Guid bookId);
}

public class BookRecord(Book book, uint inventoryCount)
{
    public Book Book { get; set; } = book;
    public uint InventoryCount { get; set; } = inventoryCount;
}