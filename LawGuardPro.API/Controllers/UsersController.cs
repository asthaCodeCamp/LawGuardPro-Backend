using MediatR;
using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.Features.Identity.Commands;

namespace LawGuardPro.API.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UsersController : ControllerBase
{

    private readonly ISender _sender;
    public UsersController(ISender sender)
    {
        _sender = sender;   
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationCommand model)
    {
        var result = await _sender.Send(model);
        if (!result.IsSuccess())
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand model)
    {
        var result = await _sender.Send(model);
        if (!result.IsSuccess())
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }           
    
}
