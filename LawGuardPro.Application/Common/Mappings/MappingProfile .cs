using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Application.Features.Cases.Commands;
using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCaseCommand, Case>();
            CreateMap<Case, CaseDto>();
            CreateMap<Lawyer, LawyerDTO>().ReverseMap();
        }
    }
}
