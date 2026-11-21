using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Results;

namespace LibrarySystemWebAPI.Repositories
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<PaginatedList<Author>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm);
    }
}