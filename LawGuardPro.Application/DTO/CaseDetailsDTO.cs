using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Application.DTO
{
    public class CaseDetailsDTO
    {
        public int CaseId { get; set; }
        public string? CaseNumber { get; set; }
        public string? CaseName { get; set; }
        public string? CaseType { get; set; }
        public string? Description { get; set; }
        public CaseStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public int TotalQuoted { get; set; }
        public int TotalPaid { get; set; }
        public Guid? UserId { get; set; }

    }
}