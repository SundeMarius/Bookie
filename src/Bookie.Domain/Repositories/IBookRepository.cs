using Bookie.Domain.Books;

namespace Bookie.Domain.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookQueryResult>> Find(BookQuery bookQuery);
    Task<BookQueryResult?> UpdateBookCount(Guid bookId, uint newCount);
    Task<Book?> Create(Book book);
    Task<BookQueryResult?> Get(Guid bookId);
    Task<Book?> Update(Book book);
    Task<Book?> Delete(Guid bookId);
}

public record BookQuery(string? BookTitle = null,
                        uint? Group = null,
                        uint? Publisher = null,
                        uint? Title = null,
                        DateOnly? From = null,
                        DateOnly? To = null);

public record BookQueryResult(Book Book, uint InventoryCount);