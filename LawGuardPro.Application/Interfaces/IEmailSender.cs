using MimeKit;
using MailKit.Net.Smtp;

namespace LawGuardPro.Application.Interfaces;

public interface IEmailSender : ISmtpClient
{
    Task<bool> SendEmailAsync(MimeMessage email);
}
