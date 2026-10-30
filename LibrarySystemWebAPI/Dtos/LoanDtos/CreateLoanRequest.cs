namespace LibrarySystemWebAPI.Dtos.LoanDtos
{
    public class CreateLoanRequest
    {
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
    }
}
