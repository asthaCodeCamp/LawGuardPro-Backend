using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LawGuardPro.Domain.Entities;

public class Email
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FromName { get; set; } = string.Empty;
    [Required]
    public string FromEmail { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
    [Required]
    public string ToEmail { get; set; } = string.Empty;
    public bool IsSent { get; set; } = false;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

