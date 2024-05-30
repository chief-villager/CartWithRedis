using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cartingWithRedis.IRepository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _set;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<T>();

        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new NullReferenceException("entity cannot be null");
            }
            await _set.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }



        public async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);

        }
        public async Task UpdateAsync(int id, T entity)
        {
            var existingEntity = await _set.FindAsync(id);
            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}