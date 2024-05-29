using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Features.Cases.Commands
{
    public class CreateCaseCommand : IRequest<Result<int>>
    {
        public string? CaseName { get; set; }
        public string? CaseType { get; set; }
        public string? Description { get; set; }
        public string? Attachment { get; set; }
        public string? UserId { get; set; }
    }
}
