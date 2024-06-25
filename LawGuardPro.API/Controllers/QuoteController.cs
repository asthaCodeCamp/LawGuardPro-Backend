using LawGuardPro.Application.Features.Quotation.Queries;
using LawGuardPro.Application.Features.Quotes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LawGuardPro.API.Controllers;

[Route("api/quote")]
[ApiController]
public class QuoteController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuoteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Invalid command." });
        }

        var result = await _mediator.Send(command);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("quote_list")]
    public async Task<IActionResult> GetQuotesByUserIdAndCaseId(Guid caseId)
    {
        var query = new GetAllQuotesByUserIdAndCaseIdQuery(caseId);
        var result = await _mediator.Send(query);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeQuoteStatus(Guid quoteId, [FromBody] ChangeQuoteStatusCommand command)
    {
        if (quoteId != command.QuoteId)
        {
            return BadRequest("Quote ID mismatch.");
        }

        var result = await _mediator.Send(command);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }
}