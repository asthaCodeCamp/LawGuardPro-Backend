using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T model);
   Task< IEnumerable<T>> FindAllAsync();
    Task UpdateAsync(T model);
    Task DeleteAsync(T model);
}