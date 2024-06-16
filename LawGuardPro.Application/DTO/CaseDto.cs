using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Application.DTO;

public class CaseDto
{
    public Guid CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public string? CaseName { get; set; }
    public CaseStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class PaginatedCaseListDto
{
    public IEnumerable<CaseDto> Cases { get; set; } = [];
    public int TotalCount { get; set; }
    public int OpenCase { get; set; }
    public int ClosedCase { get; set; }
}