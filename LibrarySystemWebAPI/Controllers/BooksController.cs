
using AutoMapper;
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Dtos.BookDtos;
using LibrarySystemWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Get All Books
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _dbContext.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .ToListAsync();

            var booksResponse =
                _mapper.Map<List<GetBookResponse>>(books);

            return Ok(booksResponse);
        }

        // Get Book By Id
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _dbContext.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            var bookResponse = _mapper.Map<GetBookResponse>(book);

            return Ok(bookResponse);
        }

        // Create Book
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(request.AuthorId);

            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = request.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book
            );
        }

        // Update Book
        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            var book = await _dbContext.Books.FindAsync(request.Id);

            if (book is null)
                return NotFound($"Book with Id = {request.Id} is not found.");

            var author = await _dbContext.Authors.FindAsync(request.AuthorId);

            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.AuthorId = request.AuthorId;
            book.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // Delete Book (Soft Delete)
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            book.IsDeleted = true;
            book.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
