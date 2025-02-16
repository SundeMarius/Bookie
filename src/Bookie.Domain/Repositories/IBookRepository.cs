using Bookie.Domain.Books;

namespace Bookie.Domain.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookInventory>> FindAsync(BookQuery bookQuery);
    Task<BookInventory?> UpdateBookCountAsync(Guid bookId, uint newCount);
    Task<Book?> CreateAsync(Book book);
    Task<BookInventory?> GetAsync(Guid bookId);
    Task<Book?> UpdateAsync(Book book);
    Task<Book?> DeleteAsync(Guid bookId);
}

public record BookQuery(string? BookTitle = null,
                        int? Group = null,
                        int? Publisher = null,
                        int? Title = null,
                        DateOnly? From = null,
                        DateOnly? To = null);

public record BookInventory(Book Book, uint InventoryCount);