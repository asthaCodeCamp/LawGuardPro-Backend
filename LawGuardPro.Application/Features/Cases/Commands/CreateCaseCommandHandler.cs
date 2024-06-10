using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LawGuardPro.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class CreateCaseCommandHandler : IRequestHandler<CreateCaseCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCaseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateCaseCommand request, CancellationToken cancellationToken)
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

        caseEntity.Status = "working";
        caseEntity.CreatedOn = DateTime.UtcNow;
        caseEntity.LastUpdated = DateTime.UtcNow;
        caseEntity.IsAttachmentAvailable = !string.IsNullOrEmpty(request.Attachment);
        caseEntity.IsLawyerAssigned = false;

        if (!string.IsNullOrEmpty(request.CaseType))
        {
            var lawyers = await _unitOfWork.LawyerRepository.GetLawyersByTypeAsync(request.CaseType);
            if (lawyers.Any())
            {
                var random = new Random();
                var lawyer = lawyers.OrderBy(x => random.Next()).First();
                caseEntity.LawyerId = lawyer.LawyerId;
                caseEntity.IsLawyerAssigned = true;
            }
        }

        await _unitOfWork.CaseRepository.AddAsync(caseEntity);
        await _unitOfWork.CommitAsync();

        return Result<int>.Success(caseEntity.CaseId);
    }
}
