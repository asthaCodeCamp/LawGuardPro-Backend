using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using LawGuardPro.Application.DTO;


namespace LawGuardPro.Application.Features.Cases.Commands
{
    public class GetCasesByUserIdQueryHandler : IRequestHandler<GetCasesByUserIdQuery, Result<(IEnumerable<CaseDto> Cases, int TotalCount)>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCasesByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<(IEnumerable<CaseDto> Cases, int TotalCount)>> Handle(GetCasesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var (cases, totalCount) = await _unitOfWork.CaseRepository.GetCasesByUserIdAsync(request.UserId, request.PageNumber, request.PageSize);

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                LastUpdated = c.LastUpdated,
                Status = c.Status
            }).ToList();

            return Result<(IEnumerable<CaseDto> Cases, int TotalCount)>.Success((caseDtos, totalCount));
        }
    }
}
