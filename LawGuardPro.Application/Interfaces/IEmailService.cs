using LawGuardPro.Application.DTO;

namespace LawGuardPro.Application.Interfaces;

public interface IEmailService{
    Task SendEmailAsync(EmailRequestDTO emailRequest);
}

