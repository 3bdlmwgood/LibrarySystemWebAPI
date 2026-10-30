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

            public LoansController(ApplicationDBContext dbContext)
            {
                _dbContext = dbContext;
            }

            // Get All Loans
            [HttpGet("GetAllLoans")]
            public IActionResult GetAllLoans()
            {
                var loans = _dbContext.Loans
                    .Include(l => l.Book)
                    .Include(l => l.Borrower)
                    .ToList();

                List<GetLoanResponse> loansResponse = new();

                foreach (var loan in loans)
                {
                    var loanResponse = new GetLoanResponse
                    {
                        BookId = loan.BookId,
                        BookTitle = loan.Book.Title,
                        BorrowerId = loan.BorrowerId,
                        BorrowerName = loan.Borrower.Name,
                        LoanDate = loan.LoanDate,
                        ReturnDate = loan.ReturnDate
                    };

                    loansResponse.Add(loanResponse);
                }

                return Ok(loansResponse);
            }

            // Borrow Book
            [HttpPost("BorrowBook")]
            public IActionResult BorrowBook(CreateLoanRequest request)
            {
                var book = _dbContext.Books.Find(request.BookId);

                if (book is null)
                    return NotFound("Book is not found.");

                var borrower = _dbContext.Borrowers.Find(request.BorrowerId);

                if (borrower is null)
                    return NotFound("Borrower is not found.");

                var existingLoan = _dbContext.Loans
                    .Find(request.BookId, request.BorrowerId);

                if (existingLoan is not null)
                    return BadRequest("This borrower already has a loan record for this book.");

                var loan = new Loan
                {
                    BookId = request.BookId,
                    BorrowerId = request.BorrowerId,
                    LoanDate = DateTime.Now,
                    ReturnDate = null
                };

                _dbContext.Loans.Add(loan);

                _dbContext.SaveChanges();

                return Ok("Book borrowed successfully.");
            }

            // Return Book
            [HttpPut("ReturnBook")]
            public IActionResult ReturnBook(int bookId, int borrowerId)
            {
                var loan = _dbContext.Loans
                    .Find(bookId, borrowerId);

                if (loan is null)
                    return NotFound("Loan record is not found.");

                if (loan.ReturnDate is not null)
                    return BadRequest("This book has already been returned.");

                loan.ReturnDate = DateTime.Now;

                _dbContext.SaveChanges();

                return Ok("Book returned successfully.");
            }
        
    }
    }
