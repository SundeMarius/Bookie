using Bookie.Application.Customers;
using Bookie.Application.Customers.Create;
using Bookie.Application.Customers.Delete;
using Bookie.Application.Customers.Get;
using FluentMonads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ISender mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> Get(Guid id)
        {
            return Ok(await mediator.Send(new GetCustomerByIdQuery(id)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Find([FromQuery] FindCustomerQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand request)
        {
            var result = await mediator.Send(request);
            return result.Match<CustomerDto, ObjectResult>(
                onSuccess: Ok,
                onFailure: Conflict
            );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteCustomerCommand(id));
            return result.Match<object, ObjectResult>(
                onSuccess: Ok,
                onFailure: NotFound
            );
        }
    }
}
