using LawGuardPro.Application.Common;
using MediatR;

namespace LawGuardPro.Application.Features.Quotes.Commands;

public class GenerateQuoteInvoiceCommand : IRequest<IResult<byte[]>>
{
    public Guid QuoteId { get; set; }

    public GenerateQuoteInvoiceCommand(Guid quoteId)
    {
        QuoteId = quoteId;
    }
}