using Bookie.Application.Abstractions;
using Bookie.Domain.Books;
using Bookie.Domain.Library;
using FluentMonads;
using FluentValidation;

namespace Bookie.Application.Library.Inventory;

public class UpdateInventoryCommandHandler(IBookRepository bookRepository) : ICommandHandler<UpdateInventoryCommand, BookRecordDto>
{
    public async Task<Result<BookRecordDto>> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = new UpdateInventoryCommandValidator().Validate(request);
        if (!validationResult.IsValid)
            return new ValidationError(validationResult.ToString());

        var newBookRecord = await bookRepository.UpdateBookCountAsync(request.BookId, request.NewCount);
        if (newBookRecord is null)
            return LibraryError.BookNotFound(request.BookId);

        return BookRecordDto.ToBookRecordDto(newBookRecord);
    }
}

public record UpdateInventoryCommand(Guid BookId, int NewCount) : ICommand<BookRecordDto>;

class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(command => command.BookId).NotEmpty();
        RuleFor(command => command.NewCount).NotNull().GreaterThanOrEqualTo(0);
    }
}
