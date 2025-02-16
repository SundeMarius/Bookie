using Bookie.Application.Abstractions;
using Bookie.Domain.Repositories;
using FluentMonads;

namespace Bookie.Application.Books.Delete;

public class DeleteBookCommandHandler(IBookRepository bookRepository) : ICommandHandler<DeleteBookCommand, BookDto>
{
    public async Task<Result<BookDto>> Handle(DeleteBookCommand command, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.DeleteAsync(command.BookId);
        if (book is null)
            return BookErrors.NotFound(command.BookId);
        return BookDto.FromBook(book);
    }
}

public record DeleteBookCommand(Guid BookId) : ICommand<BookDto>;
