using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using MediatR;

namespace LawGuardPro.Application.Features.Identity.Commands;

public class UserRegistrationCommand : IRequest<Result<UserDTO>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CountryResidency { get; set; } = string.Empty;
}

public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, Result<UserDTO>>
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

    public async Task<Result<UserDTO>> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameUnique = await _identityService.IsUniqueUser(request.Email);

        if (!isUserNameUnique)
        {
            return Result<UserDTO>.Failure(new List<Error>() { new Error() { Message = "user already exists", Code = "409 Conflict" } });
        }

        return await _identityService.RegisterAsync(_mapper.Map<RegistrationRequestDTO>(request));

    }
}

