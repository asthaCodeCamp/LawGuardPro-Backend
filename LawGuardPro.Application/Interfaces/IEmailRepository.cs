using LawGuardPro.Domain.Entities;

namespace LawGuardPro.Application.Interfaces;

public interface IEmailRepository: IRepository<Email>
{
    public IQueryable GetAllUnsentEmail();
}

