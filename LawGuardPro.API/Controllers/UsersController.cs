using LawGuardPro.Application.Common;
using LawGuardPro.Application.Features.Identity.Commands;
using LawGuardPro.Application.Features.Settings.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    [Route("api/sendemail")]
    public async Task<IActionResult> SendEmail(SendEmailCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("UpdateUserInfo")]
    public async Task<IActionResult> UpdateUserInfo(ProfileEditCommand model)
    {
        if (model == null)
        {
            return BadRequest("Invalid user data.");
        }
        var result = await _sender.Send(model);
        if (!result.IsSuccess()) return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}
