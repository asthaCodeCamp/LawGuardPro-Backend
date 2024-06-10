﻿using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Interfaces
{
    public interface ICaseRepository : IRepository<Case>
    {
        Task<List<Case>> GetCasesWithLawyersAndUsersAsync();
        Task<Case> GetCaseWithDetailsAsync(int caseId);
        Task<Case> GetCaseWithDetailsExplicitAsync(int caseId);
        Task<string> GetMaxCaseNumberAsync();
        Task<(IEnumerable<CaseDto> Cases, int TotalCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    }
}
