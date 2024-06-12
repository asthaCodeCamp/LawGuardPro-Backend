using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Entities;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class GetCasesByUserIdQuery : IRequest<IResult<(IEnumerable<Case> Cases, int TotalCount)>>
{
    public Guid UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetCasesByUserIdQuery(Guid userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}