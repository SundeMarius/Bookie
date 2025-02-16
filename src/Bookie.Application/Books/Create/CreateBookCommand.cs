using Bookie.Application.Abstractions;
using Bookie.Domain.Books;
using Bookie.Domain.Repositories;
using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Application.Books.Create;

public class CreateBookCommandHandler(IBookRepository bookRepository) : ICommandHandler<CreateBookCommand, BookDto>
{
    public async Task<Result<BookDto>> Handle(CreateBookCommand command, CancellationToken cancellationToken = default)
    {
        return await ISBN10
            .Create(command.Group, command.Publisher, command.Title)
            .AndThen(isbn10 => Book.Create(command.BookTitle, command.Author, command.ReleaseDate, isbn10))
            .AndThenAsync<Book, BookDto>(async book =>
            {
                var existingBooks = await bookRepository.FindAsync(
                    new BookQuery(
                        Group: command.Group,
                        Publisher: command.Publisher,
                        Title: command.Title));
                if (existingBooks.Any())
                    return BookErrors.AlreadyExists(book.ISBN10);
                await bookRepository.CreateAsync(book);
                return BookDto.FromBook(book);
            });
    }
}

public record CreateBookCommand(string BookTitle, string Author, DateOnly ReleaseDate, int Group, int Publisher, int Title) : ICommand<BookDto>;