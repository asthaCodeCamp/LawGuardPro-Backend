using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Application.Interfaces;

public interface IQuoteRepository : IRepository<Quote>
{
    Task<string?> GetMaxQuoteNumberAsync();
    Task<string?> GetMaxQuoteNumberByCaseIdAsync(Guid caseId);
    Task<List<Quote>> GetQuotesByCaseIdAsync(Guid caseId);
    Task<List<Quote>> GetQuotesByUserIdAndCaseIdAsync(Guid userId, Guid caseId);
    Task<Quote?> GetByIdAsync(Guid quoteId);
}