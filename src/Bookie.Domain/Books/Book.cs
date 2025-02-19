using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;
using FluentMonads;

namespace Bookie.Domain.Books;

public sealed class Book : Entity
{
    public static Result<Book> Create(
        string title,
        string author,
        DateOnly release,
        ISBN10 iSBN10,
        AuthorizationLevel minimumAuthoization = AuthorizationLevel.None)
    {
        if (string.IsNullOrEmpty(title))
            return BookErrors.EmptyTitle;

        if (string.IsNullOrEmpty(author))
            return BookErrors.EmptyAuthor;

        return new Book(title, author, release, iSBN10, minimumAuthoization);
    }

    public string Title { get; private set; }
    public string Author { get; private set; }
    public DateOnly ReleaseDate { get; private set; }
    public AuthorizationLevel MinimumAuthorization { get; private set; }
    public ISBN10 ISBN10 { get; private set; }

    private Book(string title, string author, DateOnly release, ISBN10 iSBN10, AuthorizationLevel level) : base(Guid.NewGuid())
    {
        Title = title;
        Author = author;
        ReleaseDate = release;
        ISBN10 = iSBN10;
        MinimumAuthorization = level;
    }
}

public static class BookErrors
{
    public static DomainError EmptyTitle => new("BookErrors.EmptyTitle", "The book title of has to be set");
    public static DomainError EmptyAuthor => new("BookErrors.EmptyAuthor", "The author has to be set");
}
