using MediatR;
using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class CreateCaseCommand : IRequest<IResult<int>>
{
    public string? CaseName { get; set; }
    public string? CaseType { get; set; }
    public string? Description { get; set; }
    public string? Attachment { get; set; }
    public Guid? UserId { get; set; }
}
