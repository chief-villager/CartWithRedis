using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cartingWithRedis.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int Id);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(int Id, T t);
        Task DeleteAsync(int Id);
    }
}