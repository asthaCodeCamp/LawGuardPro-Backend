using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LawGuardPro.Application.Interfaces;

public class EmailSenderService : BackgroundService
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EmailSenderService(ILogger<EmailSenderService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Email Sender Service started!");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                    List<Email> emailsToSend = emailRepository.GetAllUnsentEmail().ToList();

                    foreach (Email email in emailsToSend)
                    {
                        if (await emailService.SendEmailAsync(mapper.Map<EmailMetaData>(email)))
                        {
                            email.IsSent = true;
                            await emailRepository.UpdateAsync(email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending emails");
            }
            await Task.Delay(3000, cancellationToken);
        }
    }
}

