using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using LawGuardPro.Application.Features.Cases.Commands;

namespace LawGuardPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCaseCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess())
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCasesByUserId(string userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var query = new GetCasesByUserIdQuery(userId, pageNumber, pageSize);
            var result = await _mediator.Send(query);

            if (result.IsSuccess())
            {
                return Ok(new
                {
                    Cases = result.Data.Cases,
                    TotalCount = result.Data.TotalCount
                });
            }

            return BadRequest(new
            {
                StatusCode = result.StatusCode,
                Errors = result.Errors
            });
        }
    }
}
