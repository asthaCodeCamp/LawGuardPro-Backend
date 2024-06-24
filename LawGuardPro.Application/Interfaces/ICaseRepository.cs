using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.DTO;

namespace LawGuardPro.Application.Interfaces;

public interface ICaseRepository : IRepository<Case>
{
    Task<List<Case>> GetCasesWithLawyersAndUsersAsync();
    Task<Case?> GetCaseWithDetailsAsync(Guid caseId);
    Task<Case?> GetCaseWithDetailsExplicitAsync(Guid caseId);
    Task<string?> GetMaxCaseNumberAsync();
    Task<(IEnumerable<CaseDto?> Cases, int TotalCount, int TotalOpenCount, int TotalClosedCount)> GetCasesByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    Task<CaseDetailsDTO?> GetCaseByUserIdAndCaseIdAsync(Guid userId, Guid caseId);
    Task<Case?> GetByIdAsync(Guid caseId);
    Task<List<Attachment>> GetAttachmentsByCaseIdAsync(Guid caseId, int pageNumber, int pageSize);
}
