namespace LawGuardPro.Application.Settings;

public sealed class AppSettings
{
    public string FrontendBaseUrl { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
}