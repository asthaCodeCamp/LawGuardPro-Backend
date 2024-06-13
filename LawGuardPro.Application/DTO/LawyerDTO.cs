namespace LawGuardPro.Application.DTO;

public class LawyerDTO
{
    public Guid LawyerId { get; set; }
    public string? LawyerName { get; set; }
    public string? LawyerType { get; set; }
    public decimal Rating { get; set; }

}
