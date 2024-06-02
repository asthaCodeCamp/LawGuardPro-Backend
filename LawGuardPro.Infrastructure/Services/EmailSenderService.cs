using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LawGuardPro.Infrastructure.Services;

public class EmailSenderService : BackgroundService
{
    readonly ILogger<EmailSenderService> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailRepository _emailRepository;
    private readonly IMapper _mapper;
    public EmailSenderService(ILogger<EmailSenderService> logger, IEmailService emailService, IEmailRepository emailRepository, IMapper mapper)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRepository = emailRepository;
        _mapper = mapper;
    }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Email Sender Services start!");
            try
            {
                var emailsToSend =  _emailRepository.GetAllUnsentEmail();

                foreach (Email email in emailsToSend)
                {
                    if( await _emailService.SendEmailAsync(_mapper.Map<EmailMetaData>(email)))
                    {
                        email.IsSent = true;
                        await _emailRepository.UpdateAsync(email);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending emails");
            }
            await Task.Delay(3000,  cancellationToken);
            
        }
    }
}
