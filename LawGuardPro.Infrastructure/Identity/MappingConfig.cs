using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LawGuardPro.Application.DTO;


namespace LawGuardPro.Infrastructure
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
