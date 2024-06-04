namespace LawGuardPro.Domain.Entities;

public class Case
{
    public int CaseId { get; set; }
    public int CaseNumber { get; set; }
    public string? CaseName { get; set; }
    public string? CaseType { get; set; }
    public string? Description { get; set; }
    public bool IsAttachmentAvailable { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsLawyerAssigned { get; set; }
    public Guid? ApplicationUserId { get; set; }
    public int? LawyerId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }
    public Lawyer? Lawyer { get; set; }
}