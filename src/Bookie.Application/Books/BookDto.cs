using Bookie.Domain.Books;

namespace Bookie.Application.Books;

public record BookDto(
    Guid Id,
    string BookTitle,
    string Author,
    DateOnly Released,
    string ISBN10)
{
    public static BookDto FromBook(Book book) => new BookDto(
        Id: book.Id,
        BookTitle: book.Title,
        Author: book.Author,
        Released: book.ReleaseDate,
        ISBN10: book.ISBN10.ToString()
    );
}
