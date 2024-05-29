using LawGuardPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Case> CaseRepository { get; }
        IRepository<Lawyer> LawyerRepository { get; }
        Task<int> CommitAsync();
    }
}
