using LawGuardPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Interfaces
{
    public interface ILawyerRepository : IRepository<Lawyer>
    {
        Task<IEnumerable<Lawyer>> GetLawyersByTypeAsync(string lawyerType);
        Task<Lawyer> GetLawyerByIdAsync(int lawyerId);
    }
}
