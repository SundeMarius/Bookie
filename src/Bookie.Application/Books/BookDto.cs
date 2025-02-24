using Bookie.Domain.Authorization;
using Bookie.Domain.Books;

namespace Bookie.Application.Books;

public record BookDto(
    Guid Id,
    string BookTitle,
    string Author,
    DateOnly Published,
    AuthorizationLevel MinimumAuthorization,
    string ISBN10)
{
    public static BookDto ToBookDto(Book book) => new(
        Id: book.Id,
        BookTitle: book.Title,
        Author: book.Author,
        Published: book.Published,
        MinimumAuthorization: book.MinimumAuthorization,
        ISBN10: book.ISBN10.ToString()
    );
}