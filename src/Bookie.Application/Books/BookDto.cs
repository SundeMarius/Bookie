using Bookie.Domain.Authorization;
using Bookie.Domain.Books;

namespace Bookie.Application.Books;

public record BookDto(
    Guid Id,
    string BookTitle,
    string Author,
    DateOnly Released,
    AuthorizationLevel MinimumAuthorization,
    string ISBN10)
{
    public static BookDto ToBookDto(Book book) => new(
        Id: book.Id,
        BookTitle: book.Title,
        Author: book.Author,
        Released: book.ReleaseDate,
        MinimumAuthorization: book.MinimumAuthorization,
        ISBN10: book.ISBN10.ToString()
    );
}