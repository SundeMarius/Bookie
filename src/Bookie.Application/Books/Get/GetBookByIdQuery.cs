using Bookie.Application.Abstractions;
using Bookie.Domain.Books;

namespace Bookie.Application.Books.Get;

public class GetBookByIdQueryHandler(IBookRepository bookRepository) : IQueryHandler<GetBookByIdQuery, BookDto?>
{
    public async Task<BookDto?> Handle(GetBookByIdQuery query, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetAsync(query.Id);
        if (book is null)
            return null;
        return BookDto.FromBook(book);
    }
}

public record GetBookByIdQuery(Guid Id) : IQuery<BookDto?>;
