using MediatR;
using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.Features.Users.Queries;
using LawGuardPro.Application.Features.Users.Commands;

namespace LawGuardPro.API.Controllers;

[Route("api/address")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create/residence")]
    public async Task<IActionResult> Create([FromBody] CreateAddressResidenceCommand model)
    {
        var result = await _mediator.Send(model);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("create/billing")]
    public async Task<IActionResult> CreateBillingAddress([FromBody] CreateAddressBillingCommand model)
    {
        var result = await _mediator.Send(model);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("billing/{userId:Guid}")]
    public async Task<IActionResult> GetBillingAddressById(Guid userId)
    {
        var result = await _mediator.Send(new GetAddressResidenceQuery { UserId = userId });
        if (result == null) return NotFound();
        return Ok(result);

    }

    [HttpGet("residence/{userId:Guid}")]
    public async Task<IActionResult> GetResidencAddressById(Guid userId)
    {
        var result = await _mediator.Send(new GetAddressResidenceQuery { UserId = userId });
        if (result == null) return NotFound();
        return Ok(result);
    }
}