using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Users.Commands;
using LawGuardPro.Application.Features.Users.Queries;
using MediatR;
using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create/residence")]
        public async Task<IActionResult> Create([FromBody] CreateAddressResidenCommand model)
        {

            var result = await _mediator.Send(model);

            if (result.IsSuccess())
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }

        [HttpPost("create/billing")]
        public async Task<IActionResult> CreateBillingAddress([FromBody] CreateAddressBillingCommand model)
        {

            var result = await _mediator.Send(model);

            if (result.IsSuccess())
            {
                return Ok(result); 
            }

            return BadRequest(result);
        }

        [HttpGet("billing/{userId:Guid}")]
        public async Task<IActionResult> GetBillingAddressById(Guid userId)
        {
            var query = new GetAddressBillingQuery
            {

                UserId = userId
            };

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpGet("residence/{userId:Guid}")]
        public async Task<IActionResult> GetResidencAddressById( Guid userId)
        {
            var query = new GetAddressResidenQuery
            {

                UserId = userId

            };

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
