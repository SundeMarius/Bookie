using Bookie.Domain.Books;

namespace Bookie.Infrastructure.Memory;

public class BookRepository : IBookRepository
{
    private readonly Dictionary<Guid, BookRecord> _books = [];

    public async Task<IEnumerable<BookRecord>> FindAsync(BookQuery bookQuery)
    {
        var it = _books.Values.AsEnumerable();
        if (bookQuery.BookTitle is not null)
            it = it.Where(br => br.Book.Title == bookQuery.BookTitle);
        if (bookQuery.MinimumAuthorization is not null)
            it = it.Where(br => br.Book.MinimumAuthorization >= bookQuery.MinimumAuthorization);
        if (bookQuery.Group is not null)
            it = it.Where(br => br.Book.ISBN10.Group == bookQuery.Group);
        if (bookQuery.Publisher is not null)
            it = it.Where(br => br.Book.ISBN10.Publisher == bookQuery.Publisher);
        if (bookQuery.Title is not null)
            it = it.Where(br => br.Book.ISBN10.Title == bookQuery.Title);
        if (bookQuery.From is not null)
            it = it.Where(br => br.Book.ReleaseDate >= bookQuery.From);
        if (bookQuery.To is not null)
            it = it.Where(br => br.Book.ReleaseDate <= bookQuery.To);

        return await Task.FromResult(it);
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

    public async Task<BookRecord?> DecrementBookCountAsync(Guid bookId)
    {
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        bookRecord.InventoryCount--;
        return await Task.FromResult(bookRecord);
    }

    public async Task<BookRecord?> IncrementBookCountAsync(Guid bookId)
    {
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        bookRecord.InventoryCount++;
        return await Task.FromResult(bookRecord);
    }
}
