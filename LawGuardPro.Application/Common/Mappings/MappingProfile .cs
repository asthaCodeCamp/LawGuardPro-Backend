using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Features.Cases.Commands;

namespace LawGuardPro.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCaseCommand, Case>();
        CreateMap<Case, CaseDto>();
        CreateMap<Lawyer, LawyerDTO>().ReverseMap();
        CreateMap<AddressRequestResidencDTO, Address>();
        CreateMap<AddressRequestBillingDTO, Address>();
        CreateMap<Address, AddressResponseBillingDTO>();
        CreateMap<Address, AddressResponseResidencDTO>();
    }
}