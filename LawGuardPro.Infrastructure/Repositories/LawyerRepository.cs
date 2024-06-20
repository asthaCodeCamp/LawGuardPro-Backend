using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LawGuardPro.Infrastructure.Repositories;

public class LawyerRepository : Repository<Lawyer>, ILawyerRepository
{
    public LawyerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Lawyer>> GetLawyersByTypeAsync(string lawyerType)
    {
        return await _context.Lawyers
                             .Where(l => l.LawyerType == lawyerType)
                             .ToListAsync();
    }

    public async Task<Lawyer?> GetLawyerByIdAsync(Guid lawyerId)
    {
        return await _context.Lawyers
                             .FirstOrDefaultAsync(l => l.LawyerId == lawyerId);
    }
    public async Task<Lawyer?> GetLawyerByUserIdAndCaseIdAsync(Guid userId, Guid caseId)
    {
        return await _context.Cases
            .Where(c => c.UserId == userId && c.CaseId == caseId)
            .Select(c => c.Lawyer)
            .FirstOrDefaultAsync();
    }

}