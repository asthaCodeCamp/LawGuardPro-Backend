using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Repositories
{
    public class LawyerRepository : Repository<Lawyer>, ILawyerRepository
    {
        public LawyerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lawyer>> GetLawyersByTypeAsync(string lawyerType)
        {
            return await _context.Lawyers
                                 .Where(l => l.LawyerType == lawyerType)
                                 .ToListAsync();
        }

        public async Task<Lawyer> GetLawyerByIdAsync(int lawyerId)
        {
            return await _context.Lawyers
                                 .FirstOrDefaultAsync(l => l.LawyerId == lawyerId);
        }
    }
}

