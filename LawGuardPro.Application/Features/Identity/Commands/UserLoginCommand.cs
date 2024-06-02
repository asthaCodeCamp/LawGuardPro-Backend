using LawGuardPro.Application.DTO;
using MediatR;
using System.Net;
using System.Reflection;
using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Features.Identity.Interfaces;

namespace LawGuardPro.Application.Features.Identity.Commands;

public class UserLoginCommand : IRequest<Result<LoginResponseDTO>>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Result<LoginResponseDTO>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UserLoginCommandHandler(IMapper mapper, IIdentityService identityService)
    {
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<Result<LoginResponseDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(_mapper.Map<LoginRequestDTO>(request));
    }
}
