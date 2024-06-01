using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Users.Commands;
using LawGuardPro.Application.Features.Users.Queries;
using MediatR; 

namespace LawGuardPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateResidenceAddress")]
        public async Task<IActionResult> Create([FromBody] AddressRequestResidencDTO model)
        {
            var command = new CreateAddressResidenCommand
            {
                AddressType = 1,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Town = model.Town,
                PostalCode = model.PostalCode,
                Country = model.Country,
                UserId = 1

            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess())
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }

        [HttpPost("CreateBillingAddress")]
        public async Task<IActionResult> CreateBillingAddress([FromBody] AddressRequestBillingDTO model)
        {
            var command = new CreateAddressBillingCommand
            {
                AddressType = 2,
                BillingName=model.BillingName,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Town = model.Town,
                PostalCode = model.PostalCode,
                Country = model.Country,
                UserId = 1

            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess())
            {
                return Ok(result); 
            }

            return BadRequest(result);
        }

        [HttpGet("GetBillingAddressById")]
        public async Task<IActionResult> GetBillingAddressById([FromBody] int Id)
        {
            var query = new GetAddressBillingQueries
            {

                UserId = Id

            };

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpGet("GetResidencAddressById")]
        public async Task<IActionResult> GetResidencAddressById([FromBody] int Id)
        {
            var query = new GetAddressResidenQueries
            {

                UserId = Id

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
