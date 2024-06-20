using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetLawyerByUserIdAndCaseIdQuery : IRequest<IResult<LawyerDTO>>
{
    public Guid CaseId { get; set; }

    public GetLawyerByUserIdAndCaseIdQuery(Guid caseId)
    {
        CaseId = caseId;
    }
}