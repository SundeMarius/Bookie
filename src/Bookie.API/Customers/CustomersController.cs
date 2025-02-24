using Bookie.Application.Abstractions;
using Bookie.Application.Customers;
using Bookie.Application.Customers.Create;
using Bookie.Application.Customers.Delete;
using Bookie.Application.Customers.Get;
using Bookie.Application.Library.Loan.Create;
using Bookie.Application.Library.Rental.Get;
using FluentMonads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookie.API.Customers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand request)
    {
        var result = await mediator.Send(request);

        if (result.IsFailureAnd(e => e is ValidationError))
            return BadRequest(result.Error);

        return result.Match<CustomerDto, ObjectResult>(
            onSuccess: c => CreatedAtAction(nameof(Create), c),
            onFailure: Conflict
        );
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> Get(Guid customerId)
    {
        return Ok(await mediator.Send(new GetCustomerByIdQuery(customerId)));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Find([FromQuery] FindCustomerQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult> Delete(Guid customerId)
    {
        var result = await mediator.Send(new DeleteCustomerCommand(customerId));
        return result.Match<object, ObjectResult>(
            onSuccess: Ok,
            onFailure: NotFound
        );
    }

    [HttpPost("{customerId}/loan")]
    public async Task<ActionResult<LoanDto>> CreateLoan(Guid customerId, [FromBody] CreateLoanRequest request)
    {
        var result = await mediator.Send(new CreateLoanCommand(customerId, request.BookId, request.From, request.To));

        if (result.IsFailureAnd(e => e is ValidationError))
            return BadRequest(result.Error);

        return result.Match<LoanDto, ObjectResult>(
            onSuccess: r => CreatedAtAction(nameof(CreateLoan), r),
            onFailure: NotFound
        );
    }

    [HttpGet("{customerId}/loan")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans(Guid customerId)
    {
        return Ok(await mediator.Send(new GetLoansForCustomerQuery(customerId)));
    }
}