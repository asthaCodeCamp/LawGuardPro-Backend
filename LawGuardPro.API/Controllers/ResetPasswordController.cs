using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LawGuardPro.Application.Features.Users.Commands;

namespace LawGuardPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResetPasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ResetPasswordCommand model)
        {
            var result = await _mediator.Send(model);
            return result.IsSuccess() ? Ok(result) : BadRequest(result);
        }
    }
}
