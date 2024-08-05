using Microsoft.EntityFrameworkCore;
using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL.Repositories.Implementations
{

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SyncDbContext _dbContext;

        protected GenericRepository(SyncDbContext context)
        {
            _dbContext = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
