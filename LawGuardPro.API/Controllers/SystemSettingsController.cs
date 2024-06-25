using LawGuardPro.Application.Features.Settings.Support.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawGuardPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISender _sender;

        public SystemSettingsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("support/email")]
        public async Task<IActionResult> Support(SupportEmailCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess() ? Ok(result) : BadRequest(result);
        }
    }
}
