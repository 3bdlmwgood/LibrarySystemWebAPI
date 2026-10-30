using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Dtos.AuthorDtos;
using LibrarySystemWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public AuthorsController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get All Authors
        [HttpGet("GetAllAuthors")]
        public IActionResult GetAllAuthors()
        {
            var authors = _dbContext.Authors.ToList();

            List<GetAuthorResponse> authorsResponse = new();

            foreach (var author in authors)
            {
                var authorResponse = new GetAuthorResponse
                {
                    AuthorId = author.Id,
                    AuthorName = author.Name,
                    BirthDate = author.BirthDate
                };

                authorsResponse.Add(authorResponse);
            }

            return Ok(authorsResponse);
        }

        // Get Author By Id
        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var author = _dbContext.Authors.Find(id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            var authorResponse = new GetAuthorResponse
            {
                AuthorId = author.Id,
                AuthorName = author.Name,
                BirthDate = author.BirthDate
            };

            return Ok(authorResponse);
        }

        // Create Author
        [HttpPost]
        public IActionResult Create(CreateAuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                BirthDate = request.BirthDate
            };

            _dbContext.Authors.Add(author);

            _dbContext.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.Id },
                author
            );
        }

        // Update Author
        [HttpPut]
        public IActionResult Update(UpdateAuthorRequest request)
        {
            var author = _dbContext.Authors.Find(request.Id);

            if (author is null)
                return NotFound($"Author with Id = {request.Id} is not found.");

            author.Name = request.Name;
            author.BirthDate = request.BirthDate;

            _dbContext.SaveChanges();

            return Ok();
        }

        // Delete Author
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            var author = _dbContext.Authors.Find(id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            _dbContext.Authors.Remove(author);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}