using MediatR;
using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LawGuardPro.Application.Features.Settings.Profiles;

public class ProfileEditCommand : IRequest<IResult<UserUpdateDTO>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ProfileEditCommandHandler : IRequestHandler<ProfileEditCommand, IResult<UserUpdateDTO>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public ProfileEditCommandHandler(IMapper mapper, IIdentityService identityService)
    {
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<IResult<UserUpdateDTO>> Handle(ProfileEditCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.UpdateUserInfoAsync(_mapper.Map<UserUpdateDTO>(request));
    }
}


