using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.Domain.Entities;

public class ApplicationUser : IdentityUser{
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string PhoneNumber { get; set; }
     public string CountryResidency { get; set; }
}