using System.Linq.Expressions;

namespace LawGuardPro.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T model);
    Task<IEnumerable<T>> FindAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);
    Task UpdateAsync(T model);
    Task DeleteAsync(T model);
    IQueryable<T> FilterBy(params Expression<Func<T, bool>>[] predicate);
}