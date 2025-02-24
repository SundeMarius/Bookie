using Bookie.Application.Abstractions;
using Bookie.Domain.Authorization;
using Bookie.Domain.Books;
using Bookie.Domain.Library;
using FluentMonads;
using FluentValidation;

namespace Bookie.Application.Books.Create;

public class CreateBookCommandHandler(LibraryService library) : ICommandHandler<CreateBookCommand, BookDto>
{
    public async Task<Result<BookDto>> Handle(CreateBookCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = new CreateBookCommandValidator().Validate(command);
        if (!validationResult.IsValid)
            return new ValidationError(validationResult.ToString());

        return (await ISBN10
            .Create(command.Group, command.Publisher, command.Title)
            .AndThen(isbn10 => Book.Create(
                command.BookTitle,
                command.Author,
                command.Published,
                isbn10,
                command.MinimumAuthorization))
            .AndThenAsync(library.AddBook))
            .Map(BookDto.ToBookDto);
    }
}

public record CreateBookCommand(
    string BookTitle,
    string Author,
    DateOnly Published,
    AuthorizationLevel MinimumAuthorization,
    int Group,
    int Publisher,
    int Title
    ) : ICommand<BookDto>;

class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(createBookCommand => createBookCommand.BookTitle).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.Author).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.Published).NotEmpty();
        RuleFor(createBookCommand => createBookCommand.MinimumAuthorization).IsInEnum();
        RuleFor(createBookCommand => createBookCommand.Group).GreaterThan(0);
        RuleFor(createBookCommand => createBookCommand.Publisher).GreaterThan(0);
        RuleFor(createBookCommand => createBookCommand.Title).GreaterThan(0);
    }
}