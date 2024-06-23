using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Domain.Common.Enums;
using MediatR;

namespace LawGuardPro.Application.Features.Quotes.Commands;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, IResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public CreateQuoteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IResult<string>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContext.UserId;
            if (userId == null)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "User not logged in.", Code = "Unauthorized" } });
            }

            // Check if the user has the provided CaseId
            var caseEntity = await _unitOfWork.CaseRepository.GetCaseByUserIdAndCaseIdAsync(userId.Value, request.CaseId);
            if (caseEntity == null)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "Invalid Case ID or case does not belong to the user.", Code = "InvalidCaseId" } });
            }

            // Check if the case is assigned to the provided LawyerId
            if (caseEntity.LawyerId != request.LawyerId)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "Case is not assigned to the provided lawyer.", Code = "InvalidLawyerForCase" } });
            }

            // Check if the case is open or closed
            if (caseEntity.Status == CaseStatus.Closed)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "Cannot create a quote for a closed case.", Code = "CaseClosed" } });
            }

            // Get the maximum quote number for the provided CaseId to determine the next quote number
            var maxQuoteNumberString = await _unitOfWork.QuoteRepository.GetMaxQuoteNumberByCaseIdAsync(request.CaseId);
            int maxQuoteNumber = 0;

            if (!string.IsNullOrEmpty(maxQuoteNumberString))
            {
                var parts = maxQuoteNumberString.Split(' ');
                if (parts.Length == 3 && int.TryParse(parts[2], out int quoteNumber))
                {
                    maxQuoteNumber = quoteNumber;
                }
            }

            int nextQuoteNumber = maxQuoteNumber + 1;
            string nextQuoteNumberString = $"Quote no. {nextQuoteNumber}";

            var quote = _mapper.Map<Quote>(request);

            quote.QuoteId = Guid.NewGuid();
            quote.QuoteNumber = nextQuoteNumberString;
            quote.Status = QuoteStatus.Unpaid;
            quote.CreatedOn = DateTime.UtcNow;
            quote.UserId = userId;
            quote.PaymentMethod = "Stripe";
            quote.Value = request.Value;
            quote.TotalValue = request.TotalValue;
            quote.LawyerId = request.LawyerId;
            quote.CaseId = request.CaseId;

            // Calculate the TotalQuoted as the summation of all quotations' total value for the given case
            var existingQuotes = await _unitOfWork.QuoteRepository.GetQuotesByCaseIdAsync(request.CaseId);
            quote.TotalQuoted = existingQuotes.Sum(q => q.TotalValue) + request.TotalValue;

            // Calculate the TotalPaid as the summation of all quotations' total value for the given case with status Paid
            quote.TotalPaid = existingQuotes.Where(q => q.Status == QuoteStatus.Paid).Sum(q => q.TotalValue);

            await _unitOfWork.QuoteRepository.AddAsync(quote);
            await _unitOfWork.CommitAsync();

            return Result<string>.Success("New Quotation Created");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new List<Error> { new Error { Message = ex.Message, Code = "ServerError" } });
        }
    }
}