using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.Application.Features.Settings.Profiles;

public class ProfileInfoQuery : IRequest<IResult<UserUpdateDTO>>
{
    public string Email { get; set; } = string.Empty;
}

public class ProfileInfoQueryHandler : IRequestHandler<ProfileInfoQuery, IResult<UserUpdateDTO>>
{
    
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    public ProfileInfoQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IResult<UserUpdateDTO>> Handle(ProfileInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null )
        {
            return Result<UserUpdateDTO>.Failure(new List<Error> { new Error() { Message = "Invalid Email", Code = "InvalidEmail" } });
        }
        return Result<UserUpdateDTO>.Success(_mapper.Map<UserUpdateDTO>(user));
    }
}

