using Microsoft.EntityFrameworkCore;
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseEntity
    {
        private readonly ApplicationDBContext _dbContext;

        public GenericRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var existingEntity = await GetByIdAsync(entity.Id);

            if (existingEntity is null)
                return false;

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                return false;

            // Soft Delete
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}