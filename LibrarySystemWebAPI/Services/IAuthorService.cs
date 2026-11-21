using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Results;

namespace LibrarySystemWebAPI.Services
{
    public interface IAuthorService
    {
        Task<GenericResult<Author>> GetByIdAsync(int id);

        Task<GenericResult<PaginatedList<Author>>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm);

        Task<GenericResult<Author>> CreateAsync(Author author);

        Task<GenericResult<bool>> UpdateAsync(Author author);

        Task<GenericResult<bool>> DeleteAsync(int id);
    }
}