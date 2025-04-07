using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(params object[] keyValues);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<int> SaveChangesAsync();
    }

}
