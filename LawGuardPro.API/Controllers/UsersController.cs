using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Features.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LawGuardPro.Application.Features.Settings.Profiles;

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
        if (!result.IsSuccess()) return BadRequest(result);
        
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand model)
    {
        var result = await _sender.Send(model);
        if (!result.IsSuccess()) return StatusCode(result.StatusCode, result);
        
        return Ok(result);
    }
    [HttpPatch("UpdateUserInfo")]
    public async Task<IActionResult> UpdateUserInfo(ProfileEditCommand model)
    {
        if (model == null){
            return BadRequest("Invalid user data.");
        }
        var result = await _sender.Send(model);
        if (!result.IsSuccess()) return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}
