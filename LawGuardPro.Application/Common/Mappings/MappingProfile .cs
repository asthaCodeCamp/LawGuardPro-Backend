using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Features.Cases.Commands;
using LawGuardPro.Application.Features.Quotes.Commands;
using LawGuardPro.Application.Features.Attachments.Commands;

namespace LawGuardPro.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCaseCommand, Case>();
        CreateMap<Case, CaseDto>();
        CreateMap<Case, CaseDetailsDTO>();
        CreateMap<Lawyer, LawyerDTO>().ReverseMap();
        CreateMap<AddressRequestResidencDTO, Address>();
        CreateMap<AddressRequestBillingDTO, Address>();
        CreateMap<Address, AddressResponseBillingDTO>();
        CreateMap<Address, AddressResponseResidenceDTO>();
        CreateMap<CreateQuoteCommand, Quote>();
        CreateMap<Quote, QuoteDTO>();
        CreateMap<SaveAttachmentCommand, Attachment>();
        CreateMap<Attachment, AttachmentDto>();
    }
}