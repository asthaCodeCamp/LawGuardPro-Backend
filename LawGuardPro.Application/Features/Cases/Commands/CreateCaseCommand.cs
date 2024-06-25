using MediatR;
using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Features.Cases.Commands;

public class CreateCaseCommand : IRequest<IResult<Guid>>
{
    public string CaseName { get; set; }
    public string CaseType { get; set; }
    public string Description { get; set; }
}