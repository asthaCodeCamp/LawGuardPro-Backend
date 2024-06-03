﻿using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Commands;
using LawGuardPro.Domain.Entities;

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
            CreateMap<EmailMetaData, Email>().ReverseMap();
            CreateMap<EmailMetaData, SendEmailCommand>().ReverseMap();
        }
        catch (Exception ex)
        {
        }
    }
}
