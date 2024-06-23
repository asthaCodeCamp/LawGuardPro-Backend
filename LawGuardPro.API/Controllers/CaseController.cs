using Microsoft.AspNetCore.Mvc;
using MediatR;
using LawGuardPro.Application.Features.Cases.Queries;
using LawGuardPro.Application.Features.Cases.Commands;

namespace LawGuardPro.API.Controllers;

[Route("api/case")]
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

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCasesByUserId([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var query = new GetCasesByUserIdQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("{caseId}")]
    public async Task<IActionResult> GetCaseByUserIdAndCaseId(Guid caseId)
    {
        var query = new GetCaseByUserIdAndCaseIdQuery(caseId);
        var result = await _mediator.Send(query);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeCaseStatus(Guid caseId, [FromBody] ChangeCaseStatusCommand command)
    {
        if (caseId != command.CaseId)
        {
            return BadRequest("Case ID mismatch.");
        }

        var result = await _mediator.Send(command);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }
}