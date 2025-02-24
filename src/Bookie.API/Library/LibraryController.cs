using Bookie.Application.Abstractions;
using Bookie.Application.Books.Get;
using Bookie.Application.Library;
using Bookie.Application.Library.Inventory;
using FluentMonads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Library;

[Route("api/[controller]")]
[ApiController]
public class LibraryController(ISender mediator) : ControllerBase
{
    [HttpGet("{bookId}")]
    public async Task<ActionResult<BookRecordDto>> Get(Guid bookId)
    {
        var bookRecord = await mediator.Send(new GetBookByIdQuery(bookId));
        return Ok(bookRecord);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookRecordDto>>> Find([FromQuery] FindBookQuery query)
    {
        var bookRecords = await mediator.Send(query);
        return Ok(bookRecords);
    }

    [HttpPost]
    public async Task<ActionResult<BookRecordDto>> UpdateInventory([FromBody] UpdateInventoryRequest request)
    {
        var result = await mediator.Send(new UpdateInventoryCommand(request.BookId, request.NewCount));

        if (result.IsFailureAnd(e => e is ValidationError))
            return BadRequest(result.Error);

        return result.Match<BookRecordDto, ObjectResult>(
            onSuccess: Ok,
            onFailure: NotFound
        );
    }
}
