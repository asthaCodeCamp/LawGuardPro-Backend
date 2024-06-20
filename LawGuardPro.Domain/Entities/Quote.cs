using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Domain.Entities;

public class Quote
{
    public Guid? QuoteId { get; set; }
    public string? QuoteNumber { get; set; }
    public string? Value { get; set; }
    public string? TotalValue { get; set; }
    public DateTime CreatedOn { get; set; }
    public QuoteStatus Status { get; set; }
    public string? PaymentMethod { get; set; }
    public Guid? UserId { get; set; }
    public Guid? LawyerId { get; set; }
    public Guid? CaseId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }
    public Lawyer? Lawyer { get; set; }
    public Case? Case { get; set; }
}
