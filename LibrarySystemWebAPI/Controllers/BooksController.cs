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

        public BooksController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get All Books
        [HttpGet("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var books = _dbContext.Books
                .Include(b => b.Author)
                .ToList();

            List<GetBookResponse> booksResponse = new();

            foreach (var book in books)
            {
                var bookResponse = new GetBookResponse
                {
                    BookId = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    AuthorId = book.AuthorId,
                    AuthorName = book.Author.Name
                };

                booksResponse.Add(bookResponse);
            }

            return Ok(booksResponse);
        }

        // Get Book By Id
        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var book = _dbContext.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            var bookResponse = new GetBookResponse
            {
                BookId = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name
            };

            return Ok(bookResponse);
        }

        // Create Book
        [HttpPost]
        public IActionResult Create(CreateBookRequest request)
        {
            var author = _dbContext.Authors.Find(request.AuthorId);

            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = request.AuthorId
            };

            _dbContext.Books.Add(book);

            _dbContext.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book
            );
        }

        // Update Book
        [HttpPut]
        public IActionResult Update(UpdateBookRequest request)
        {
            var book = _dbContext.Books.Find(request.Id);

            if (book is null)
                return NotFound($"Book with Id = {request.Id} is not found.");

            var author = _dbContext.Authors.Find(request.AuthorId);

            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.AuthorId = request.AuthorId;

            _dbContext.SaveChanges();

            return Ok();
        }

        // Delete Book
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            var book = _dbContext.Books.Find(id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            _dbContext.Books.Remove(book);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
