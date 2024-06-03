using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.Domain.Entities;

public class ApplicationUser: IdentityUser<Guid> {
     public string FirstName { get; set; } = string.Empty;
     public string LastName { get; set; } = string.Empty;
     public string CountryResidency { get; set; } = string.Empty;
     public ICollection<Address> AddressUsers { get; set; } = new List<Address>();
}
