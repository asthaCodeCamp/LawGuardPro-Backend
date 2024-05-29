using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Commands;
using LawGuardPro.Application.Features.Settings.Profiles;

namespace LawGuardPro.Infrastructure;

public class MappingConfig: Profile
{
    public MappingConfig()
    {
        try
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<UserLoginCommand, LoginRequestDTO>().ReverseMap();
            CreateMap<RegistrationRequestDTO, UserRegistrationCommand>().ReverseMap();
            CreateMap<UserUpdateDTO, ProfileEditCommand>().ReverseMap();

        }
        catch (Exception ex)
        {
        }
    }
}
