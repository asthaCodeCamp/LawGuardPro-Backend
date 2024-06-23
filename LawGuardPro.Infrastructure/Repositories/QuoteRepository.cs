using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LawGuardPro.Infrastructure.Repositories;

public class QuoteRepository : Repository<Quote>, IQuoteRepository
{
    public QuoteRepository(ApplicationDbContext context) : base(context) { }

    public async Task<string?> GetMaxQuoteNumberAsync()
    {
        return await _context.Quotes
            .OrderByDescending(q => q.QuoteNumber)
            .Select(q => q.QuoteNumber)
            .FirstOrDefaultAsync();
    }
    public async Task<string?> GetMaxQuoteNumberByCaseIdAsync(Guid caseId)
    {
        return await _context.Quotes
            .Where(q => q.CaseId == caseId)
            .OrderByDescending(q => q.QuoteNumber)
            .Select(q => q.QuoteNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Quote>> GetQuotesByCaseIdAsync(Guid caseId)
    {
        return await _context.Quotes
            .Where(q => q.CaseId == caseId)
            .ToListAsync();
    }
    public async Task<List<Quote>> GetQuotesByUserIdAndCaseIdAsync(Guid userId, Guid caseId)
    {
        return await _context.Quotes
            .Where(q => q.UserId == userId && q.CaseId == caseId)
            .ToListAsync();
    }
    public async Task<Quote?> GetByIdAsync(Guid quoteId)
    {
        return await _context.Quotes.FindAsync(quoteId);
    }
}