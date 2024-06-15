using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Commands;
using LawGuardPro.Application.Features.Settings.Profiles;
using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Infrastructure;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        try
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<UserLoginCommand, LoginRequestDTO>().ReverseMap();
            CreateMap<RegistrationRequestDTO, UserRegistrationCommand>().ReverseMap();
            CreateMap<EmailMetaData, Email>().ReverseMap();
            CreateMap<EmailMetaData, SendEmailCommand>().ReverseMap();
            CreateMap<ApplicationUser, AddressResponseBillingDTO>().ReverseMap();
            CreateMap<ApplicationUser, AddressResponseResidenceDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserUpdateDTO>().ReverseMap();
            CreateMap<ProfileEditCommand, UserUpdateDTO>().ReverseMap();
        }
        catch (Exception ex)
        {
        }
    }
}
