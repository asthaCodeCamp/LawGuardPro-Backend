using MediatR;
using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Features.Quotes.Commands;

public class CreateQuoteCommand : IRequest<IResult<string>>
{
    public int Value { get; set; }
    public int TotalValue { get; set; }
    public Guid LawyerId { get; set; }
    public Guid CaseId { get; set; }
}