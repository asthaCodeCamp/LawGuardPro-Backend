using LawGuardPro.Application.DTO;
using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMetaData emailMetaData);
    Task AddEmailToQueueAsync(EmailMetaData emailMetaData);
}