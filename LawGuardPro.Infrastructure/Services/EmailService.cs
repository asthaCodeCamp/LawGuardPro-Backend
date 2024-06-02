using MimeKit;
using AutoMapper;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.DTO;
using Microsoft.Extensions.Options;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure.Settings;

namespace LawGuardPro.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IEmailRepository _emailRepository;
    private readonly IMapper _mapper;
    public EmailService(IOptions<SmtpSettings> smtpSettings, IEmailRepository emailRepository,IMapper mapper)
    {
        _smtpSettings = smtpSettings.Value;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    public async Task<bool> SendEmailAsync(EmailMetaData emailMetaData)
    {
        try
        {
            var email = new EmailBuilder()
                .SetFrom(_smtpSettings.FromName, _smtpSettings.FromEmail)
                .SetTo(emailMetaData.To)
                .SetSubject(emailMetaData.Subject)
                .SetBody(emailMetaData.Body)
                .Build();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            Console.WriteLine("Email sent successfully to " + emailMetaData.To);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.Message);
            return false;
        }
    }



    public async Task AddEmailToQueueAsync(EmailMetaData emailMetaDataDto)
    {
        await _emailRepository.AddAsync(_mapper.Map<Email>(emailMetaDataDto));
    }
}
