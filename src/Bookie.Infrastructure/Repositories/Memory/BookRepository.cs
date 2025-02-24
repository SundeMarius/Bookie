using Bookie.Domain.Authorization;
using Bookie.Domain.Books;

namespace Bookie.Infrastructure.Repositories.Memory;

public class BookRepository : IBookRepository
{
    private readonly Dictionary<Guid, BookRecord> _books = [];

    public async Task<IEnumerable<BookRecord>> FindAsync(
                        string? bookTitle = null,
                        string? author = null,
                        int? group = null,
                        int? publisher = null,
                        int? title = null,
                        AuthorizationLevel? minimumAuthorization = null,
                        DateOnly? from = null,
                        DateOnly? to = null)
    {
        var it = _books.Values.AsEnumerable();
        if (bookTitle is not null)
            it = it.Where(br => br.Book.Title.Contains(bookTitle, StringComparison.CurrentCultureIgnoreCase));
        if (author is not null)
            it = it.Where(br => br.Book.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase));
        if (minimumAuthorization is not null)
            it = it.Where(br => br.Book.MinimumAuthorization >= minimumAuthorization);
        if (group is not null)
            it = it.Where(br => br.Book.ISBN10.Group == group);
        if (publisher is not null)
            it = it.Where(br => br.Book.ISBN10.Publisher == publisher);
        if (title is not null)
            it = it.Where(br => br.Book.ISBN10.Title == title);
        if (from is not null)
            it = it.Where(br => br.Book.Published >= from);
        if (to is not null)
            it = it.Where(br => br.Book.Published <= to);

        return await Task.FromResult(it);
    }

    public async Task<BookRecord?> GetBookRecordAsync(Guid bookId)
    {
        if (_books.TryGetValue(bookId, out var bookRecord))
            return await Task.FromResult(bookRecord);
        return null;
    }

    public async Task<BookRecord?> UpdateBookCountAsync(Guid bookId, int newCount)
    {
        if (newCount < 0)
            throw new ArgumentOutOfRangeException(nameof(newCount), "The new count has to be non negative");
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        bookRecord.InventoryCount = (uint)newCount;
        return await Task.FromResult(bookRecord);
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

    public async Task<Book> UpdateAsync(Book book)
    {
        _books[book.Id].Book = book;
        return await Task.FromResult(book);
    }

    public async Task<Book?> DeleteAsync(Guid bookId)
    {
        if (!_books.TryGetValue(bookId, out var bookRecord))
            return null;
        _books.Remove(bookId);
        return await Task.FromResult(bookRecord.Book);
    }
}
