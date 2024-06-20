using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Queries;

public class GetLawyerByUserIdAndCaseIdQueryHandler : IRequestHandler<GetLawyerByUserIdAndCaseIdQuery, IResult<LawyerDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetLawyerByUserIdAndCaseIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IResult<LawyerDTO>> Handle(GetLawyerByUserIdAndCaseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var lawyer = await _unitOfWork.LawyerRepository.GetLawyerByUserIdAndCaseIdAsync(_userContext.UserId!.Value, request.CaseId);

            if (lawyer == null)
            {
                return Result<LawyerDTO>.Failure(new List<Error> { new Error { Message = "Lawyer not found", Code = "NotFound" } });
            }

            var lawyerDto = _mapper.Map<LawyerDTO>(lawyer);

            return Result<LawyerDTO>.Success(lawyerDto);
        }
        catch (Exception ex)
        {
            return Result<LawyerDTO>.Failure(new List<Error> { new Error { Message = ex.Message, Code = "ServerError" } });
        }
    }
}