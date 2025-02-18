using Bookie.Application.Abstractions;
using Bookie.Domain.Library;
using FluentMonads;

namespace Bookie.Application.Books.Delete;

public class DeleteBookCommandHandler(LibraryService library) : ICommandHandler<DeleteBookCommand>
{
    public async Task<Result> Handle(DeleteBookCommand command, CancellationToken cancellationToken = default)
    {
        return await library.DeleteBook(command.BookId);
    }
}

public record DeleteBookCommand(Guid BookId) : ICommand;
