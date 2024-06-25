using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Quotation.Queries;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;
using MediatR;

namespace LawGuardPro.Application.Features.Quotes.Queries;

public class GetAllQuotesByUserIdAndCaseIdQueryHandler : IRequestHandler<GetAllQuotesByUserIdAndCaseIdQuery, IResult<QuoteListDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetAllQuotesByUserIdAndCaseIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IResult<QuoteListDTO>> Handle(GetAllQuotesByUserIdAndCaseIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (userId == null)
        {
            return Result<QuoteListDTO>.Failure(new List<Error> { new Error { Message = "User not logged in.", Code = "Unauthorized" } });
        }

        var caseEntity = await _unitOfWork.CaseRepository.GetCaseWithDetailsAsync(request.CaseId);

        if (caseEntity == null || caseEntity.UserId != userId)
        {
            return Result<QuoteListDTO>.Failure(new List<Error> { new Error { Message = "Case not found or does not belong to the user.", Code = "InvalidCase" } });
        }

        var quotes = await _unitOfWork.QuoteRepository.GetQuotesByUserIdAndCaseIdAsync(userId.Value, request.CaseId);
        var quoteDTOs = _mapper.Map<IEnumerable<QuoteDTO>>(quotes);

        var totalQuoted = quotes.Sum(q => q.TotalValue);
        var totalPaid = quotes.Where(q => q.Status == QuoteStatus.Paid).Sum(q => q.TotalValue);

        var quoteListDTO = new QuoteListDTO
        {
            Quotes = quoteDTOs,
            TotalQuoted = totalQuoted,
            TotalPaid = totalPaid,
            UserId = userId,
            LawyerId = caseEntity.LawyerId,
            CaseId = caseEntity.CaseId,
            CaseNumber = caseEntity.CaseNumber,
            CaseName = caseEntity.CaseName,
            Status = caseEntity.Status,
            LastUpdated = caseEntity.LastUpdated
        };

        return Result<QuoteListDTO>.Success(quoteListDTO);
    }
}