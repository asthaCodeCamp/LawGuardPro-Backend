using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;
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

    public async Task<Case?> GetCaseWithDetailsAsync(Guid caseId)
    {
        return await _context.Cases
            .Include(c => c.ApplicationUser)
            .Include(c => c.Lawyer)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CaseId == caseId);
    }

    public async Task<Case?> GetCaseWithDetailsExplicitAsync(Guid caseId)
    {
        var caseEntity = await _context.Cases.AsNoTracking().FirstOrDefaultAsync(c => c.CaseId == caseId);
        if (caseEntity != null)
        {
            await _context.Entry(caseEntity).Reference(c => c.ApplicationUser).LoadAsync();
            await _context.Entry(caseEntity).Reference(c => c.Lawyer).LoadAsync();
        }
        return caseEntity;
    }

    public async Task<string?> GetMaxCaseNumberAsync()
    {
        var maxCaseNumber = await _context.Cases
           .OrderByDescending(c => c.CaseNumber)
           .Select(c => c.CaseNumber)
           .FirstOrDefaultAsync();

        return maxCaseNumber;
    }

    public async Task<(IEnumerable<Case?> Cases, int TotalCount, int TotalOpenCount, int TotalClosedCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize)
    {
        var query = _context.Cases
            .Where(c => c.UserId == userId)
            .Select(c => new Case
            {
                CaseId = c.CaseId,
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                Status = c.Status,
                LastUpdated = c.LastUpdated
            });

        var totalCount = await query.CountAsync();

        var openCasesQuery = _context.Cases
    .Where(c => c.UserId == userId && c.Status == CaseStatus.Working)
    .Select(c => new Case
    {
        CaseId = c.CaseId,
        CaseNumber = c.CaseNumber,
        CaseName = c.CaseName,
        Status = c.Status,
        LastUpdated = c.LastUpdated
    });

        var closedCasesQuery = _context.Cases
            .Where(c => c.UserId == userId && c.Status == CaseStatus.Closed)
            .Select(c => new Case
            {
                CaseId = c.CaseId,
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                Status = c.Status,
                LastUpdated = c.LastUpdated
            });

        var totalOpenCount = await openCasesQuery.CountAsync();
        var totalClosedCount = await closedCasesQuery.CountAsync();

        var cases = await query
            .OrderByDescending(c => c.LastUpdated)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (cases, totalCount, totalOpenCount, totalClosedCount);
    }
    public async Task<Case?> GetCaseByUserIdAndCaseIdAsync(Guid userId, Guid caseId)
    {
        return await _context.Cases
            .Where(c => c.UserId == userId && c.CaseId == caseId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

}