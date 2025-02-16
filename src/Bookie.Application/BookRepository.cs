using Bookie.Domain.Books;
using Bookie.Domain.Repositories;

namespace Bookie.Application;

public class BookRepository : IBookRepository
{
    private readonly Dictionary<Guid, Book> _books = [];

    public async Task<Book?> CreateAsync(Book book)
    {
        _books.Add(book.Id, book);
        return await Task.FromResult(book);
    }

    public async Task<Book?> DeleteAsync(Guid bookId)
    {
        if (_books.TryGetValue(bookId, out var book))
        {
            _books.Remove(bookId);
            return await Task.FromResult(book);
        }
        return null;
    }

    public async Task<IEnumerable<BookInventory>> FindAsync(BookQuery bookQuery)
    {
        return await Task.FromResult(_books.Values.Where(b =>
            b.ISBN10.Group == bookQuery.Group
            && b.ISBN10.Publisher == bookQuery.Publisher
            && b.ISBN10.Title == bookQuery.Title).Select(b => new BookInventory(b, 1)));
    }

    public async Task<BookInventory?> GetAsync(Guid bookId)
    {
        if (_books.TryGetValue(bookId, out var book))
            return await Task.FromResult(new BookInventory(book, 1));
        return null;
    }

    public async Task<Book?> UpdateAsync(Book book)
    {
        _books.Add(book.Id, book);
        return await Task.FromResult(book);
    }

    public async Task<BookInventory?> UpdateBookCountAsync(Guid bookId, uint newCount)
    {
        var bookInventory = await GetAsync(bookId);
        if (bookInventory is null)
            return null;
        return bookInventory with { InventoryCount = newCount };
    }
}
