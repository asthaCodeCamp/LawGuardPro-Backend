using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;
using MediatR;

namespace LawGuardPro.Application.Features.Cases.Commands
{
    public class GetCasesByUserIdQuery : IRequest<Result<(IEnumerable<CaseDto> Cases, int TotalCount)>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetCasesByUserIdQuery(string userId, int pageNumber, int pageSize)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
