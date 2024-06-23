using LawGuardPro.Domain.Common.Enums;

namespace LawGuardPro.Application.DTO;

public class QuoteDTO
{
    public Guid? QuoteId { get; set; }
    public string? QuoteNumber { get; set; }
    public int Value { get; set; }
    public int TotalValue { get; set; }
    public DateTime CreatedOn { get; set; }
    public QuoteStatus Status { get; set; }
    public string? PaymentMethod { get; set; }
}

public class QuoteListDTO
{
    public IEnumerable<QuoteDTO> Quotes { get; set; } = [];
    public int TotalQuoted { get; set; }
    public int TotalPaid { get; set; }
    public Guid? UserId { get; set; }
    public Guid? LawyerId { get; set; }
    public Guid? CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public string? CaseName { get; set; }
    public CaseStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
}