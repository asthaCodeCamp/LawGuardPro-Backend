using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using LawGuardPro.Application.DTO;
using Microsoft.Extensions.Options;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure.Settings;
using MimeKit.Cryptography;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace LawGuardPro.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(EmailRequestDTO emailRequest)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        email.To.Add(new MailboxAddress("", emailRequest.To));

        var userName = "Shafayet";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>Hey,<br>Just wanted to say {userName} hi all the way from the land of C#.<br>-- Code guy</p>";
        bodyBuilder.TextBody = $"Just fun";
        email.Subject = "Testing out email sending";
        email.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}
