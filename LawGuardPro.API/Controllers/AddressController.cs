using MediatR;
using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.Features.Users.Queries;
using LawGuardPro.Application.Features.Users.Commands;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Application.Services;

namespace LawGuardPro.API.Controllers;

[Route("api/address")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    public AddressController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

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

    [HttpGet("get/billing")]
    public async Task<IActionResult> GetBillingAddressById()
    {
        var result = await _mediator.Send(new GetAddressBillingQuery { UserId = _userContext.UserId!.Value });
        if (result == null) return NotFound();
        return Ok(result);

    }

    [HttpGet("get/residence")]
    public async Task<IActionResult> GetResidencAddressById()
    {
        var result = await _mediator.Send(new GetAddressResidenceQuery { UserId = _userContext.UserId!.Value });
        if (result == null) return NotFound();
        return Ok(result);
    }
}