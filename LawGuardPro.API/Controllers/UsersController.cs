using LawGuardPro.API.Models;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Feature.Identity.Commands;
using LawGuardPro.Application.Feature.Identity.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LawGuardPro.API.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ISender _sender;
        protected APIResponse _response;

        public UsersController(ISender sender)
        {
            _sender = sender;
            this._response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationCommand model)
        {
            var result = await _sender.Send(model);

            if (result == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand model)
        {
            //var loginResponse = await _userRepo.Login(model);
            var result = await _sender.Send(model);
            if (result == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or password is incorrect");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Result = result;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }
    }
}
