using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Application.DTO;

public class CaseDto
{
    public int CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public string? CaseName { get; set; }
    public CaseStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }

}
