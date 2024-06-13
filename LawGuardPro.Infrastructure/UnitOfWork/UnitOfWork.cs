using LawGuardPro.Infrastructure.Persistence.Context;
using LawGuardPro.Infrastructure.Repositories;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Infrastructure.UnitofWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ICaseRepository _caseRepository;
    private ILawyerRepository _lawyerRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICaseRepository CaseRepository => _caseRepository ??= new CaseRepository(_context);
    public ILawyerRepository LawyerRepository => _lawyerRepository ??= new LawyerRepository(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
