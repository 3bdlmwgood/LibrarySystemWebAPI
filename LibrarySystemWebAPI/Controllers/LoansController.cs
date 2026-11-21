
using AutoMapper;
using LibrarySystemWebAPI.Data;
using LibrarySystemWebAPI.Dtos.LoanDtos;
using LibrarySystemWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public LoansController(ApplicationDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Get All Loans
        [HttpGet("GetAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _dbContext.Loans
                .Include(l => l.Book)
                .Include(l => l.Borrower)
                .AsNoTracking()
                .ToListAsync();

            var loansResponse =
                _mapper.Map<List<GetLoanResponse>>(loans);

            return Ok(loansResponse);
        }

        // Borrow Book
        [HttpPost("BorrowBook")]
        public async Task<IActionResult> BorrowBook(CreateLoanRequest request)
        {
            var book = await _dbContext.Books.FindAsync(request.BookId);

            if (book is null)
                return NotFound("Book is not found.");

            var borrower = await _dbContext.Borrowers.FindAsync(request.BorrowerId);

            if (borrower is null)
                return NotFound("Borrower is not found.");

            var existingLoan = await _dbContext.Loans
                .FindAsync(request.BookId, request.BorrowerId);

            if (existingLoan is not null)
                return BadRequest("This borrower already has a loan record for this book.");

            var loan = new Loan
            {
                BookId = request.BookId,
                BorrowerId = request.BorrowerId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();

            return Ok("Book borrowed successfully.");
        }

        // Return Book
        [HttpPut("ReturnBook")]
        public async Task<IActionResult> ReturnBook(int bookId, int borrowerId)
        {
            var loan = await _dbContext.Loans
                .FindAsync(bookId, borrowerId);

            if (loan is null)
                return NotFound("Loan record is not found.");

            if (loan.ReturnDate is not null)
                return BadRequest("This book has already been returned.");

            loan.ReturnDate = DateTime.UtcNow;
            loan.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok("Book returned successfully.");
        }
    }
}
