using Microsoft.EntityFrameworkCore;
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Results;

namespace LibrarySystemWebAPI.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AuthorRepository(ApplicationDBContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Author>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm)
        {
            var query = _dbContext.Authors.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var authors = await query
                .OrderBy(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Author>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)pageSize),
                Data = authors
            };
        }
    }
}