using Bookie.Application.Abstractions;
using Bookie.Application.Library;
using Bookie.Domain.Authorization;
using Bookie.Domain.Books;

namespace Bookie.Application.Books.Get;

public class FindBookQueryHandler(IBookRepository bookRepository) : IQueryHandler<FindBookQuery, IEnumerable<BookRecordDto>>
{
    public async Task<IEnumerable<BookRecordDto>> Handle(FindBookQuery request, CancellationToken cancellationToken)
    {
        return (await bookRepository.FindAsync(
            request.BookTitle,
            request.Author,
            request.Group,
            request.Publisher,
            request.Title,
            request.MinimumAuthorization,
            request.From,
            request.To
        )).Select(br => BookRecordDto.ToBookRecordDto(br));
    }
}

public record FindBookQuery(
    string? BookTitle = null,
    string? Author = null,
    int? Group = null,
    int? Publisher = null,
    int? Title = null,
    AuthorizationLevel? MinimumAuthorization = null,
    DateOnly? From = null,
    DateOnly? To = null) : IQuery<IEnumerable<BookRecordDto>>;

