using Bookie.Application.Books.Create;
using Bookie.Application.Books.Delete;
using Bookie.Application.Books.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Books;

[Route("api/[controller]")]
[ApiController]
public class BooksController(ISender mediator) : ControllerBase
{
    [HttpGet("{bookId}")]
    public async Task<IActionResult> Get(Guid bookId)
    {
        return Ok(await mediator.Send(new GetBookByIdQuery(bookId)));
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] FindBookQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand request)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Ok(result.Value) : Conflict(result.Error);
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> Delete(Guid bookId)
    {
        var result = await mediator.Send(new DeleteBookCommand(bookId));
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }
}
