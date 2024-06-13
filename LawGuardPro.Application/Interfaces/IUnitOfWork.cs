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
        ICaseRepository CaseRepository { get; }
        ILawyerRepository LawyerRepository { get; }
        Task<int> CommitAsync();
    }
}
