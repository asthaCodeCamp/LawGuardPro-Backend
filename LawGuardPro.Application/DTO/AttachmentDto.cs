namespace LawGuardPro.Application.DTO;

public class AttachmentDto
{
    public Guid AttachmentId { get; set; }
    public string Title { get; set; }
    public string FileURL { get; set; }
    public string Type { get; set; }
    public string UploadedBy { get; set; }
    public DateOnly AddedOn { get; set; }
    public Guid CaseId { get; set; }
}