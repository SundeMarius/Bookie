using Bookie.Application.Customers;
using Bookie.Application.Library.Rental.Create;
using Bookie.Application.Library.Rental.Get;
using FluentMonads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Library;

[Route("api/[controller]")]
[ApiController]
public class LibraryController(ISender mediator) : ControllerBase
{
    [HttpGet("rental/{id}")]
    public async Task<IActionResult> GetRental(Guid id)
    {
        return Ok(await mediator.Send(new GetRentalByIdQuery(id)));
    }

    [HttpPost("rental")]
    public async Task<IActionResult> AddRental(CreateRentalCommand request)
    {
        var rental = await mediator.Send(request);
        return rental.Match<RentalDto, ObjectResult>(
            onSuccess: Ok,
            onFailure: NotFound
        );
    }
}
