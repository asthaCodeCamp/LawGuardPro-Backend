using LawGuardPro.Application.DTO;
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

    public async Task<(IEnumerable<CaseDto?> Cases, int TotalCount, int TotalOpenCount, int TotalClosedCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize)
    {
        var casesQuery = _context.Cases
            .Where(c => c.UserId == userId);

        var totalCount = await casesQuery.CountAsync();

        var openCasesCount = await casesQuery
            .Where(c => c.Status == CaseStatus.Working)
            .CountAsync();

        var closedCasesCount = await casesQuery
            .Where(c => c.Status == CaseStatus.Closed)
            .CountAsync();

        var cases = await casesQuery
            .OrderByDescending(c => c.LastUpdated)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CaseDto
            {
                CaseId = c.CaseId,
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                Status = c.Status,
                LastUpdated = c.LastUpdated,
                TotalQuoted = _context.Quotes.Where(q => q.CaseId == c.CaseId).Sum(q => q.TotalValue),
                TotalPaid = _context.Quotes.Where(q => q.CaseId == c.CaseId).Sum(q => q.TotalPaid)
            })
            .ToListAsync();

        return (cases, totalCount, openCasesCount, closedCasesCount);
    }
    public async Task<CaseDetailsDTO?> GetCaseByUserIdAndCaseIdAsync(Guid userId, Guid caseId)
    {
        var caseEntity = await _context.Cases
            .Where(c => c.UserId == userId && c.CaseId == caseId)
            .AsNoTracking()
            .Select(c => new CaseDetailsDTO
            {
                CaseId = caseId,
                CaseNumber = c.CaseNumber,
                CaseName = c.CaseName,
                CaseType = c.CaseType,
                Description = c.Description,
                Status = c.Status,
                LastUpdated = c.LastUpdated,
                UserId = userId,
                LawyerId = c.LawyerId,
                TotalQuoted = _context.Quotes.Where(q => q.CaseId == c.CaseId).Sum(q => q.TotalValue),
                TotalPaid = _context.Quotes.Where(q => q.CaseId == c.CaseId).Sum(q => q.TotalPaid)
            })
            .FirstOrDefaultAsync();

        return caseEntity;
    }
    public async Task<Case?> GetByIdAsync(Guid caseId)
    {
        return await _context.Cases.FindAsync(caseId);
    }
    public async Task<List<Attachment>> GetAttachmentsByCaseIdAsync(Guid caseId, int pageNumber, int pageSize)
    {
        return await _context.Attachments
            .Where(a => a.CaseId == caseId)
            .OrderByDescending(a => a.AddedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}