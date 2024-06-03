namespace LawGuardPro.Application.DTO;

public class AddressRequestBillingDTO
{       
    public string? BillingName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }

    // Foreign key property
   
}
