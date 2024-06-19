namespace LawGuardPro.Application.Interfaces;

public interface ISmtpSettings
{
    string Server { get; set; }
    int Port { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string FromEmail { get; set; }
    string FromName { get; set; }
    bool UseSSL { get; set; }
}
