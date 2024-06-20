using LawGuardPro.Application.Common;
using LawGuardPro.Application.Features.Identity.Commands;
using LawGuardPro.Application.Features.Settings.Profiles;
using LawGuardPro.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LawGuardPro.API.Controllers;

[Route("api/usersauth")]
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

    [HttpPatch("updateuserinfo")]
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

    [HttpPost("forgetpassword")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand model)
    {
        var result = await _sender.Send(model);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("resetforgottenpassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetForgottenPasswordCommand model)
    {
        var result = await _sender.Send(model);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("getuserinfo")]
    public async Task<IActionResult> GetUserInfo(ProfileInfoQuery model)
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
