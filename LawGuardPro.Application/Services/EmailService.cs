using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Interfaces;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace LawGuardPro.Application.Services;

public class EmailService : IEmailService
{
    private readonly IEmailRepository _emailRepository;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public EmailService(
        IMapper mapper,
        IEmailSender emailSender,
        IEmailRepository emailRepository
        )
    {
        _mapper = mapper;
        _emailSender = emailSender;
        _emailRepository = emailRepository;
    }

    public async Task<bool> SendEmailAsync(EmailMetaData emailMetaData)
    {
        var email = new EmailBuilder()
            .SetFromName(emailMetaData.FromName)
            .SetFromEmail(emailMetaData.FromEmail)
            .SetToName(emailMetaData.ToName)
            .SetToEmail(emailMetaData.ToEmail)
            .SetSubject(emailMetaData.Subject)
            .SetHtmlBody(emailMetaData.Body)
            .AddAttachmentsIfHas(emailMetaData.Attachements!)
            .Build();

        return await _emailSender.SendEmailAsync(email);
    }

    public async Task AddEmailToQueueAsync(EmailMetaData emailMetaDataDto)
    {
        await _emailRepository.AddAsync(_mapper.Map<Email>(emailMetaDataDto));
    }
}