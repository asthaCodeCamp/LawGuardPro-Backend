using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Application.Features.Quotes.Commands;

public class ChangeQuoteStatusCommand : IRequest<LawGuardPro.Application.Common.IResult>
{
    public Guid QuoteId { get; set; }
    public QuoteStatus NewStatus { get; set; }
}

public class ChangeQuoteStatusCommandHandler : IRequestHandler<ChangeQuoteStatusCommand, LawGuardPro.Application.Common.IResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeQuoteStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LawGuardPro.Application.Common.IResult> Handle(ChangeQuoteStatusCommand request, CancellationToken cancellationToken)
    {
        var quoteEntity = await _unitOfWork.QuoteRepository.GetByIdAsync(request.QuoteId);

        if (quoteEntity == null)
        {
            return Result.Failure(StatusCodes.Status404NotFound, new List<Error> { new Error { Message = "Quote not found", Code = "NotFound" } });
        }

        quoteEntity.Status = request.NewStatus;

        // Update the TotalPaid
        var quotes = await _unitOfWork.QuoteRepository.GetQuotesByCaseIdAsync(quoteEntity.CaseId ?? Guid.Empty);
        quoteEntity.TotalPaid = quotes.Where(q => q.Status == QuoteStatus.Paid).Sum(q => q.TotalValue);

        await _unitOfWork.QuoteRepository.UpdateAsync(quoteEntity);
        await _unitOfWork.CommitAsync();

        return Result.Success(StatusCodes.Status200OK);
    }
}