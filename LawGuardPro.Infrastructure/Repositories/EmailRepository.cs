using LawGuardPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure.Persistence.Context;

namespace LawGuardPro.Infrastructure.Repositories;

public class EmailRepository: Repository<Email>, IEmailRepository
{
    public EmailRepository(ApplicationDbContext context) : base(context) { }

    public IQueryable<Email> GetAllUnsentEmail()
    {
        var query = from emails in _context.Emails
                    where emails.IsSent == false
                    select emails;     
        return query;
    }
}

