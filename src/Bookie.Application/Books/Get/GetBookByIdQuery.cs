using Bookie.Application.Abstractions;
using Bookie.Application.Library;
using Bookie.Domain.Books;

namespace Bookie.Application.Books.Get;

public class GetBookByIdQueryHandler(IBookRepository bookRepository) : IQueryHandler<GetBookByIdQuery, BookRecordDto?>
{
    public async Task<BookRecordDto?> Handle(GetBookByIdQuery query, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetBookRecordAsync(query.Id);
        if (book is null)
            return null;
        return BookRecordDto.ToBookRecordDto(book);
    }
}

public record GetBookByIdQuery(Guid Id) : IQuery<BookRecordDto?>;
