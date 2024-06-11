using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LawGuardPro.Infrastructure.Repositories;

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

    public async Task<Case?> GetCaseWithDetailsAsync(int caseId)
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

    public async Task<string> GetMaxCaseNumberAsync()
    {
        var maxCaseNumber = await _context.Cases
           .OrderByDescending(c => c.CaseNumber)
           .Select(c => c.CaseNumber)
           .FirstOrDefaultAsync();

        return maxCaseNumber;
    }

    public async Task<(IEnumerable<CaseDto> Cases, int TotalCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize)
    {
        var query = _context.Cases
            .Where(c => c.UserId == userId)
            .Select(c => new CaseDto
            {
                CaseId = c.CaseId,
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                Status = c.Status,
                LastUpdated = c.LastUpdated
            });

        var totalCount = await query.CountAsync();

        var cases = await query
            .OrderByDescending(c => c.LastUpdated)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (cases, totalCount);
    }

}
