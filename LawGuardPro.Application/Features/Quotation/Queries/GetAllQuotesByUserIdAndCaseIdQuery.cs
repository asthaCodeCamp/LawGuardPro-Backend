using MediatR;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Features.Quotation.Queries;

public class GetAllQuotesByUserIdAndCaseIdQuery : IRequest<IResult<QuoteListDTO>>
{
    public Guid CaseId { get; set; }

    public GetAllQuotesByUserIdAndCaseIdQuery(Guid caseId)
    {
        CaseId = caseId;
    }
}