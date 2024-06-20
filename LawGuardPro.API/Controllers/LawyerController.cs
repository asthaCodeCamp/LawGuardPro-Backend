using AutoMapper;
using LawGuardPro.Application.Features.Cases.Queries;
using LawGuardPro.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LawGuardPro.API.Controllers;

[Route("api/lawyer")]
[ApiController]
public class LawyersController : ControllerBase
{
    private readonly ILawyerRepository _lawyerRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public LawyersController(ILawyerRepository lawyerRepository, IMapper mapper, IMediator mediator)
    {
        _lawyerRepository = lawyerRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("{caseId}")]
    public async Task<IActionResult> GetLawyerByUserIdAndCaseId(Guid caseId)
    {
        var query = new GetLawyerByUserIdAndCaseIdQuery(caseId);
        var result = await _mediator.Send(query);

        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }
}