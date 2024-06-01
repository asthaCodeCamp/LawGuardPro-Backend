using Microsoft.AspNetCore.Mvc;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Users.Commands; 
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
            var command = new CreateAddressCommand
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
                return Ok(result); // Assuming you have a standard response object for success
            }

            return BadRequest(result); // Assuming you have a standard response object for failure
        }
    }
}
