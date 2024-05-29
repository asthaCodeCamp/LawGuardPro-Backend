using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Infrastructure.Repositories;
using LawGuardPro.Application;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Case> _caseRepository;
        private IRepository<Lawyer> _lawyerRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Case> CaseRepository => _caseRepository ??= new Repository<Case>(_context);
        public IRepository<Lawyer> LawyerRepository => _lawyerRepository ??= new Repository<Lawyer>(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
