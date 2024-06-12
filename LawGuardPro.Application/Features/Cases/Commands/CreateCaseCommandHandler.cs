using AutoMapper;
using LawGuardPro.Domain.Entities;
using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class CreateCaseCommandHandler : IRequestHandler<CreateCaseCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCaseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult<int>> Handle(CreateCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = _mapper.Map<Case>(request);

        var maxCaseNumberString = await _unitOfWork.CaseRepository.GetMaxCaseNumberAsync();
        int maxCaseNumber = 0;

        if (!string.IsNullOrEmpty(maxCaseNumberString))
        {
            maxCaseNumber = int.Parse(maxCaseNumberString);
        }

        int nextCaseNumber = maxCaseNumber + 1;
        caseEntity.CaseNumber = nextCaseNumber.ToString("D6");

        caseEntity.Status = CaseStatus.Working;
        caseEntity.CreatedOn = DateTime.UtcNow;
        caseEntity.LastUpdated = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(request.CaseType))
        {
            var lawyers = await _unitOfWork.LawyerRepository.GetLawyersByTypeAsync(request.CaseType);
            if (lawyers.Any())
            {
                var random = new Random();
                var lawyer = lawyers.OrderBy(x => random.Next()).First();
                caseEntity.LawyerId = lawyer.LawyerId;
            }
        }

        await _unitOfWork.CaseRepository.AddAsync(caseEntity);
        await _unitOfWork.CommitAsync();

        return Result<int>.Success(caseEntity.CaseId);
    }

}