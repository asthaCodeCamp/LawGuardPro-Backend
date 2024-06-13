namespace LawGuardPro.Domain.Entities;

public class Lawyer
{
    public int LawyerId { get; set; }
    public string? LawyerName { get; set; }
    public string? LawyerType { get; set; }
    public decimal Rating { get; set; }

    public ICollection<Case> Cases { get; set; } = new List<Case>();

}
