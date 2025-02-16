using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Domain.Books;

public sealed class Book : Entity
{
    public static Result<Book> Create(string title, string author, DateOnly release, ISBN10 iSBN10)
    {
        if (string.IsNullOrEmpty(title))
            return BookErrors.EmptyTitle;
        if (string.IsNullOrEmpty(author))
            return BookErrors.EmptyAuthor;
        return new Book(title, author, release, iSBN10);
    }

    public string Title { get; private set; }
    public string Author { get; private set; }
    public DateOnly ReleaseDate { get; private set; }
    public ISBN10 ISBN10 { get; private set; }

    private Book(string title, string author, DateOnly release, ISBN10 iSBN10) : base(Guid.NewGuid())
    {
        Title = title;
        Author = author;
        ReleaseDate = release;
        ISBN10 = iSBN10;
    }
}

public static class BookErrors
{
    public static BookieError EmptyTitle => new("BookErrors.EmptyTitle", "The book title of has to be set");
    public static BookieError EmptyAuthor => new("BookErrors.EmptyAuthor", "The author has to be set");
}
