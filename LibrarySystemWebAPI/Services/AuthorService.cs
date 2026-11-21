using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Repositories;
using LibrarySystemWebAPI.Results;

namespace LibrarySystemWebAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<GenericResult<Author>> GetByIdAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author is null)
            {
                return GenericResult<Author>.Failure(
                    $"Author with Id = {id} was not found.");
            }

            return GenericResult<Author>.Success(author);
        }

        public async Task<GenericResult<PaginatedList<Author>>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return GenericResult<PaginatedList<Author>>.Failure(
                    "Invalid page number or page size.");
            }

            var authors = await _repository.GetAllAsync(
                pageNumber, pageSize, searchTerm);

            return GenericResult<PaginatedList<Author>>.Success(authors);
        }

        public async Task<GenericResult<Author>> CreateAsync(Author author)
        {
            var createdAuthor = await _repository.CreateAsync(author);

            return GenericResult<Author>.Success(createdAuthor);
        }

        public async Task<GenericResult<bool>> UpdateAsync(Author author)
        {
            var isUpdated = await _repository.UpdateAsync(author);

            return isUpdated
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Author was not found.");
        }

        public async Task<GenericResult<bool>> DeleteAsync(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);

            return isDeleted
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Author was not found.");
        }
    }
}