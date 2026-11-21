
using AutoMapper;
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Dtos.AuthorDtos;
using LibrarySystemWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Get All Authors
        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _dbContext.Authors
                .AsNoTracking()
                .ToListAsync();

            var authorsResponse =  _mapper.Map<List<GetAuthorResponse>>(authors);

            return Ok(authorsResponse);
        }

        // Get Author By Id
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _dbContext.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            var authorResponse = _mapper.Map<GetAuthorResponse>(author);

            return Ok(authorResponse);
        }

        // Create Author
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                BirthDate = request.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.Id },
                _mapper.Map<GetAuthorResponse>(author)
            );
        }

        // Update Author
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuthorRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(request.Id);

            if (author is null)
                return NotFound($"Author with Id = {request.Id} is not found.");

            author.Name = request.Name;
            author.BirthDate = request.BirthDate;
            author.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // Delete Author (Soft Delete)
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            author.IsDeleted = true;
            author.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
