using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class ChangeCaseStatusCommand : IRequest<LawGuardPro.Application.Common.IResult>
{
    public Guid CaseId { get; set; }
    public CaseStatus NewStatus { get; set; }
}

public class ChangeCaseStatusCommandHandler : IRequestHandler<ChangeCaseStatusCommand, LawGuardPro.Application.Common.IResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeCaseStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LawGuardPro.Application.Common.IResult> Handle(ChangeCaseStatusCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await _unitOfWork.CaseRepository.GetByIdAsync(request.CaseId);

        if (caseEntity == null)
        {
            return Result.Failure(new List<Error> { new Error { Message = "Case not found", Code = "NotFound" } });
        }

        caseEntity.Status = request.NewStatus;
        await _unitOfWork.CaseRepository.UpdateAsync(caseEntity);
        await _unitOfWork.CommitAsync();

        return Result.Success(StatusCodes.Status200OK);
    }
}