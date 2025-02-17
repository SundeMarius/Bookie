using Bookie.Domain.Books;

namespace Bookie.Infrastructure.Memory;

public class BookRepository : IBookRepository
{
    private readonly Dictionary<Guid, BookRecord> _books = [];

    public async Task<IEnumerable<BookRecord>> FindAsync(BookQuery bookQuery)
    {
        return await Task.FromResult(_books.Values.Where(br =>
            br.Book.ISBN10.Group == bookQuery.Group
            && br.Book.ISBN10.Publisher == bookQuery.Publisher
            && br.Book.ISBN10.Title == bookQuery.Title));
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _books.Add(book.Id, new BookRecord(book, 0));
        return await Task.FromResult(book);
    }

    public async Task<Book?> GetAsync(Guid id)
    {
        if (_books.TryGetValue(id, out var bookRecord))
            return await Task.FromResult(bookRecord.Book);
        return null;
    }

    public async Task<BookRecord?> GetBookCountAsync(Guid bookId)
    {
        if (_books.TryGetValue(bookId, out var bookRecord))
            return await Task.FromResult(bookRecord);
        return null;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _books[book.Id].Book = book;
        return await Task.FromResult(book);
    }

    public async Task<BookRecord?> UpdateBookCountAsync(Guid bookId, uint newCount)
    {
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        bookRecord.InventoryCount = newCount;
        return await Task.FromResult(bookRecord);
    }

    public async Task<Book?> DeleteAsync(Guid bookId)
    {
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        _books.Remove(bookId);
        return await Task.FromResult(bookRecord.Book);
    }
}
