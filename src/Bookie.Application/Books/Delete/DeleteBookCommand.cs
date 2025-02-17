using Bookie.Application.Abstractions;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Books.Delete;

public class DeleteBookCommandHandler(LibraryService library) : ICommandHandler<DeleteBookCommand, BookDto>
{
    public async Task<Result<BookDto>> Handle(DeleteBookCommand command, CancellationToken cancellationToken = default)
    {
        return (await library.DeleteBook(command.BookId)).Map(BookDto.FromBook);
    }
}

public record DeleteBookCommand(Guid BookId) : ICommand<BookDto>;
