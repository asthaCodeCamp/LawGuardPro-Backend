using LawGuardPro.API.Models;
using LawGuardPro.Application.DTO;
using LawGuardPro.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LawGuardPro.API.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IIdentityService _userService;
        protected APIResponse _response;
        public UsersController(IIdentityService userService)
        {
            _userService = userService;
            this._response = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool ifUserNameUnique = _userService.IsUniqueUser(model.Email);
            if (!ifUserNameUnique)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName already exists!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var user = await _userService.Register(model);
            if (user == null)
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
    }
}
