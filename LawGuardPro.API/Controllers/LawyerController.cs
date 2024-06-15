using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LawGuardPro.API.Controllers;

[Route("api/lawyer")]
[ApiController]
public class LawyersController : ControllerBase
{
    private readonly ILawyerRepository _lawyerRepository;
    private readonly IMapper _mapper;

    public LawyersController(ILawyerRepository lawyerRepository, IMapper mapper)
    {
        _lawyerRepository = lawyerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LawyerDTO>>> GetLawyers()
    {
        var lawyers = await _lawyerRepository.FindAllAsync();
        var lawyerDTOs = _mapper.Map<IEnumerable<LawyerDTO>>(lawyers);
        return Ok(lawyerDTOs);
    }
}
