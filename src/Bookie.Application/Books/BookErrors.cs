using Bookie.Domain.Books;
using Bookie.Infrastructure;

namespace Bookie.Application.Books;

public static class BookErrors
{
    public static BookieError AlreadyExists(ISBN10 iSBN10) => new("BookError.AlreadyExists", $"The book with isbn '{iSBN10}' already exists");
    public static BookieError NotFound(Guid bookId) => new("BookError.NotFound", $"The book with id '{bookId}' does not exist");
}
