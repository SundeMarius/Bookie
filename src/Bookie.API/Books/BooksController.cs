using Bookie.Application.Books;
using Bookie.Application.Books.Create;
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
        var query = new GetBookByIdQuery(bookId);

        return Ok(await mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand request)
    {
        var result = await mediator.Send(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
