using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetCaseByUserIdAndCaseIdQuery : IRequest<IResult<CaseDetailsDTO>>
{
    public Guid CaseId { get; }

    public GetCaseByUserIdAndCaseIdQuery(Guid caseId)
    {
        CaseId = caseId;
    }
}