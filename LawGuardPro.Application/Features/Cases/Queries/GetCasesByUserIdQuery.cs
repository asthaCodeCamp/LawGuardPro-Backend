using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetCasesByUserIdQuery : IRequest<IResult<PaginatedCaseListDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetCasesByUserIdQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}