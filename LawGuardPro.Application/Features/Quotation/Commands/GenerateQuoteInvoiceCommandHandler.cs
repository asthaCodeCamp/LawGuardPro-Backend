using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Application.Services;
using MediatR;

namespace LawGuardPro.Application.Features.Quotes.Commands;

public class GenerateQuoteInvoiceCommandHandler : IRequestHandler<GenerateQuoteInvoiceCommand, IResult<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;

    public GenerateQuoteInvoiceCommandHandler(IUnitOfWork unitOfWork, IPdfService pdfService)
    {
        _unitOfWork = unitOfWork;
        _pdfService = pdfService;
    }

    public async Task<IResult<byte[]>> Handle(GenerateQuoteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var quote = await _unitOfWork.QuoteRepository.GetByIdAsync(request.QuoteId);
        if (quote == null)
        {
            return Result<byte[]>.Failure(new List<Error> { new Error { Message = "Quote not found.", Code = "NotFound" } });
        }

        var pdfBytes = _pdfService.GenerateQuoteInvoice(quote);
        return Result<byte[]>.Success(pdfBytes);
    }
}