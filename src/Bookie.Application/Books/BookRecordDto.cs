using Bookie.Domain.Books;

namespace Bookie.Application.Books;

public record BookRecordDto(BookDto Book, uint InventoryCount)
{
    public static BookRecordDto ToBookRecordDto(BookRecord bookRecord)
    {
        return new(BookDto.ToBookDto(bookRecord.Book), bookRecord.InventoryCount);
    }
}
