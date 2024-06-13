using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetCaseByUserIdAndCaseIdQueryHandler : IRequestHandler<GetCaseByUserIdAndCaseIdQuery, IResult<CaseDetailsDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCaseByUserIdAndCaseIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult<CaseDetailsDTO>> Handle(GetCaseByUserIdAndCaseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var caseEntity = await _unitOfWork.CaseRepository.GetCaseByUserIdAndCaseIdAsync(request.UserId, request.CaseId);

            if (caseEntity == null)
            {
                return Result<CaseDetailsDTO>.Failure(new List<Error> { new Error { Message = "Case not found", Code = "NotFound" } });
            }

            var caseDto = _mapper.Map<CaseDetailsDTO>(caseEntity);
            return Result<CaseDetailsDTO>.Success(caseDto);
        }
        catch (Exception ex)
        {
            return Result<CaseDetailsDTO>.Failure(new List<Error> { new Error { Message = ex.Message, Code = "ServerError" } });
        }
    }
}