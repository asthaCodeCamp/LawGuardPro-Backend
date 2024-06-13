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

        if (result.IsSuccess())
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetCasesByUserId(Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
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
    
    [HttpGet("user/{userId}/case/{caseId}")]
    public async Task<IActionResult> GetCaseByUserIdAndCaseId(Guid userId, int caseId)
    {
        var query = new GetCaseByUserIdAndCaseIdQuery(userId, caseId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess())
        {
            return Ok(result.Data);
        }

        return BadRequest(new
        {
            StatusCode = result.StatusCode,
            Errors = result.Errors
        });
    }
}