using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Application.Interfaces;

public interface ICaseRepository : IRepository<Case>
{
    Task<List<Case>> GetCasesWithLawyersAndUsersAsync();
    Task<Case?> GetCaseWithDetailsAsync(int caseId);
    Task<Case?> GetCaseWithDetailsExplicitAsync(int caseId);
    Task<string?> GetMaxCaseNumberAsync();
    Task<(IEnumerable<Case?> Cases, int TotalCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize);
}
