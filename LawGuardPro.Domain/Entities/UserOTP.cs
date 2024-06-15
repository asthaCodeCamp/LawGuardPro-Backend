using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LawGuardPro.Domain.Entities;

public class UserOTP
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UId {  get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}

