using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Services.Interfaces;
using LawGuardPro.Infrastructure.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LawGuardPro.Infrastructure.Services;

internal class EmailSender : SmtpClient, IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<bool> SendEmailAsync(MimeMessage email)
    {
        var isSent = false;

        try
        {
            await ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
            await AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await SendAsync(email);
            await DisconnectAsync(true);

            isSent = true;
        }
        catch { }
        finally { Dispose(); }

        return isSent;
    }
}
