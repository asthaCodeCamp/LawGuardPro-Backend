namespace LawGuardPro.Application.DTO;

public class AddressResponseResidencDTO
{
    public Guid UserId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string Town { get; set; }
    public string Country { get; set; }
}