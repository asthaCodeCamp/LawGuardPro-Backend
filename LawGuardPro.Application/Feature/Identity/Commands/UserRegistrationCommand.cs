using LawGuardPro.Application.DTO;
using MediatR;
using System.Net;
using System.Reflection;
using LawGuardPro.Application.Feature.Identity.Interfaces;
using AutoMapper;

namespace LawGuardPro.Application.Feature.Identity.Commands;

public class UserRegistrationCommand : IRequest<UserDTO>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CountryResidency { get; set; } = string.Empty;
}

public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserDTO>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UserRegistrationCommandHandler(
        IMapper mapper,
        IIdentityService identityService)
    {
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<UserDTO> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameUnique = await _identityService.IsUniqueUser(request.Email);

        if (!isUserNameUnique) throw new NotImplementedException();

        var user = await _identityService.Register(_mapper.Map<RegistrationRequestDTO>(request));
        return user;
    }
}

