using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Domain.Entities;

public class Case
{
    public Guid CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public string? CaseName { get; set; }
    public string? CaseType { get; set; }
    public string? Description { get; set; }
    public CaseStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdated { get; set; }
    public Guid? UserId { get; set; }
    public Guid? LawyerId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }
    public Lawyer? Lawyer { get; set; }
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}