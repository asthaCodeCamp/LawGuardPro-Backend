using MediatR;
using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;

namespace LawGuardPro.Application.Features.Settings.Profiles;

public class ProfileEditCommand : IRequest<Result<UserDTO>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

public class ProfileEditCommandHandler : IRequestHandler<ProfileEditCommand, IResult<UserDTO>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public ProfileEditCommandHandler(IMapper mapper, IIdentityService identityService)
    {
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<IResult<UserDTO>> Handle(ProfileEditCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.UpdateUserInfoAsync(_mapper.Map<UserUpdateDTO>(request));
    }
}


