using LawGuardPro.Infrastructure.Persistence.Context;

namespace LawGuardPro.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(T model)
    {
        await _context.AddAsync(model);
    }

    public Task<bool> DeleteAsync(T model)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> FindAllCaseAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(T model)
    {
        throw new NotImplementedException();
    }
}