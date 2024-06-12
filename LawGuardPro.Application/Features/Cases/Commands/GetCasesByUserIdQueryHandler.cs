using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class GetCasesByUserIdQueryHandler : IRequestHandler<GetCasesByUserIdQuery, IResult<(IEnumerable<CaseDto> Cases, int TotalCount)>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCasesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult<(IEnumerable<CaseDto> Cases, int TotalCount)>> Handle(GetCasesByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var casesResult = await _unitOfWork.CaseRepository.GetCasesByUserIdAsync(request.UserId, request.PageNumber, request.PageSize);

            var totalCount = casesResult.TotalCount;

            var pagedCases = casesResult.Cases
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var cases = pagedCases.Select(c => _mapper.Map<CaseDto>(c));

            return Result<(IEnumerable<CaseDto>, int TotalCount)>.Success((cases, totalCount));
        }
        catch (Exception ex)
        {
            return Result<(IEnumerable<CaseDto>, int TotalCount)>.Failure(new List<Error> { new Error { Message = ex.Message, Code = "ServerError" } });
        }
    }
}