using LawGuardPro.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LawGuardPro.Domain.Entities;

public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public AddressType AddressType { get; set; }
    public string? BillingName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Town { get; set; }
    public int PostalCode { get; set; }
    public string? Country { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
