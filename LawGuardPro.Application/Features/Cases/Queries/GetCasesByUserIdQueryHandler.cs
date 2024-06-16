using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetCasesByUserIdQueryHandler : IRequestHandler<GetCasesByUserIdQuery, IResult<PaginatedCaseListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetCasesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IResult<PaginatedCaseListDto>> Handle(GetCasesByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var casesResult = await _unitOfWork.CaseRepository
                .GetCasesByUserIdAsync(_userContext.UserId!.Value, request.PageNumber, request.PageSize);

            var totalCount = casesResult.TotalCount;

            var pagedCases = casesResult.Cases
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var cases = pagedCases.Select(c => _mapper.Map<CaseDto>(c));
            var openCaseCount = casesResult.TotalOpenCount;
            var closedCaseCount = casesResult.TotalClosedCount;

            return Result<PaginatedCaseListDto>.Success(new() 
            {
                Cases = cases,
                TotalCount = totalCount,
                OpenCase = openCaseCount,
                ClosedCase = closedCaseCount
            });
        }
        catch (Exception ex)
        {
            return Result<PaginatedCaseListDto>.Failure(new List<Error> { new Error { Message = ex.Message, Code = "ServerError" } });
        }
    }
}