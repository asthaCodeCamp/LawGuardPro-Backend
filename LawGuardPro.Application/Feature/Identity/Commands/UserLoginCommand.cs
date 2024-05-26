using LawGuardPro.Application.DTO;
using MediatR;
using System.Net;
using System.Reflection;
using LawGuardPro.Application.Feature.Identity.Interfaces;
using AutoMapper;

namespace LawGuardPro.Application.Feature.Identity.Commands;
public class UserLoginCommand : IRequest<LoginResponseDTO> {
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResponseDTO>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UserLoginCommandHandler(IMapper mapper, IIdentityService identityService)
    {
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<LoginResponseDTO> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var loginResponse = await _identityService.Login(_mapper.Map<LoginRequestDTO>(request));
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token)) throw new NotImplementedException();
        return loginResponse;
    }
}

