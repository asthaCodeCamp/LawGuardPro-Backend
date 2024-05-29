using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Infrastructure.Identity;

namespace LawGuardPro.Infrastructure.Repositories
{
    public class CaseRepository : Repository<Case>, ICaseRepository
    {
        public CaseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Case>> GetCasesWithLawyersAndUsersAsync()
        {
            return await _context.Cases
                .Include(c => c.ApplicationUser)
                .Include(c => c.Lawyer)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Case> GetCaseWithDetailsAsync(int caseId)
        {
            return await _context.Cases
                .Include(c => c.ApplicationUser)
                .Include(c => c.Lawyer)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CaseId == caseId);
        }

        public async Task<Case> GetCaseWithDetailsExplicitAsync(int caseId)
        {
            var caseEntity = await _context.Cases.AsNoTracking().FirstOrDefaultAsync(c => c.CaseId == caseId);
            if (caseEntity != null)
            {
                await _context.Entry(caseEntity).Reference(c => c.ApplicationUser).LoadAsync();
                await _context.Entry(caseEntity).Reference(c => c.Lawyer).LoadAsync();
            }
            return caseEntity;
        }
    }
}
