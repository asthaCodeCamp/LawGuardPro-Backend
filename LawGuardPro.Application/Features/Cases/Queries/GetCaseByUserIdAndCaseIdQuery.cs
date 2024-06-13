using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetCaseByUserIdAndCaseIdQuery : IRequest<IResult<CaseDetailsDTO>>
{
    public Guid UserId { get; }
    public int CaseId { get; }

    public GetCaseByUserIdAndCaseIdQuery(Guid userId, int caseId)
    {
        UserId = userId;
        CaseId = caseId;
    }
}