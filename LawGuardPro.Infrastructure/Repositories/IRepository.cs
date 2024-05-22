using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T model);
    IEnumerable<T> FindAllCaseAsync();
    Task<bool> UpdateAsync(T model);
    Task<bool> DeleteAsync(T model);
}