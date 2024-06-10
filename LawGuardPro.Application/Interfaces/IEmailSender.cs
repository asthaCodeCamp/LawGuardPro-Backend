using MimeKit;
using MailKit.Net.Smtp;

namespace LawGuardPro.Infrastructure.Services.Interfaces;

public interface IEmailSender : ISmtpClient
{
    Task<bool> SendEmailAsync(MimeMessage email);
}
