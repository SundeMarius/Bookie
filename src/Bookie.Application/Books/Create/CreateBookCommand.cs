using Bookie.Application.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Books;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Books.Create;

public class CreateBookCommandHandler(LibraryService library) : ICommandHandler<CreateBookCommand, BookDto>
{
    public async Task<Result<BookDto>> Handle(CreateBookCommand command, CancellationToken cancellationToken = default)
    {
        var book = await ISBN10
            .Create(command.Group, command.Publisher, command.Title)
            .AndThen(isbn10 => Book.Create(command.BookTitle, command.Author, command.ReleaseDate, isbn10, command.MinimumAuthorization))
            .AndThenAsync(library.AddBook);

        return book.Map(BookDto.FromBook);
    }
}

public record CreateBookCommand(
    string BookTitle,
    string Author,
    DateOnly ReleaseDate,
    AuthorizationLevel MinimumAuthorization,
    int Group,
    int Publisher,
    int Title
    ) : ICommand<BookDto>;