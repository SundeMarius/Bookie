using Bookie.Application.Abstractions;
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
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> Get(Guid id)
    {
        return Ok((await mediator.Send(new GetBookByIdQuery(id)))?.Book);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> Find([FromQuery] FindBookQuery query)
    {
        return Ok((await mediator.Send(query)).Select(b => b.Book));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookCommand request)
    {
        var result = await mediator.Send(request);

        if (result.IsFailureAnd(e => e is ValidationError))
            return BadRequest(result.Error);

        return result.Match<BookDto, ObjectResult>(
            onSuccess: b => CreatedAtAction(nameof(Create), b),
            onFailure: Conflict
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteBookCommand(id));
        return result.Match<object, ObjectResult>(
            onSuccess: Ok,
            onFailure: NotFound
        );
    }
}
