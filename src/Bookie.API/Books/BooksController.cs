using Bookie.Application.Books;
using Bookie.Application.Books.Create;
using Bookie.Application.Books.Delete;
using Bookie.Application.Books.Get;
using FluentMonads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Books;

[Route("api/[controller]")]
[ApiController]
public class BooksController(ISender mediator) : ControllerBase
{
    [HttpGet("{bookId}")]
    public async Task<ActionResult<BookDto>> Get(Guid bookId)
    {
        return Ok(await mediator.Send(new GetBookByIdQuery(bookId)));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookRecordDto>>> Find([FromQuery] FindBookQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<BookDto, ObjectResult>(
            onSuccess: Ok,
            onFailure: Conflict
        );
    }

    [HttpDelete("{bookId}")]
    public async Task<ActionResult> Delete(Guid bookId)
    {
        var result = await mediator.Send(new DeleteBookCommand(bookId));
        return result.Match<object, ObjectResult>(
            onSuccess: Ok,
            onFailure: NotFound
        );
    }
}
