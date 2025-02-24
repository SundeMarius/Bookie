using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;
using FluentMonads;

namespace Bookie.Domain.Books;

public sealed class Book : Entity
{
    public static Result<Book> Create(
        string title,
        string author,
        DateOnly published,
        ISBN10 iSBN10,
        AuthorizationLevel minimumAuthoization = AuthorizationLevel.None)
    {
        if (string.IsNullOrEmpty(title))
            return BookError.EmptyTitle;

        if (string.IsNullOrEmpty(author))
            return BookError.EmptyAuthor;

        return new Book(title, author, published, iSBN10, minimumAuthoization);
    }

    public string Title { get; private set; }
    public string Author { get; private set; }
    public DateOnly Published { get; private set; }
    public ISBN10 ISBN10 { get; private set; }
    public AuthorizationLevel MinimumAuthorization { get; private set; }

    private Book(string title, string author, DateOnly published, ISBN10 iSBN10, AuthorizationLevel level)
    {
        Title = title;
        Author = author;
        Published = published;
        ISBN10 = iSBN10;
        MinimumAuthorization = level;
    }
}

public static class BookError
{
    public static DomainError EmptyTitle => new("BookError.EmptyTitle", "The Book Title has to be set");
    public static DomainError EmptyAuthor => new("BookError.EmptyAuthor", "The Author has to be set");
}
