using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LawGuardPro.Application.DTO;

public class AddressResponseResidencDTO
{
    public Guid UserId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string Town { get; set; }
    public string Country { get; set; }
}