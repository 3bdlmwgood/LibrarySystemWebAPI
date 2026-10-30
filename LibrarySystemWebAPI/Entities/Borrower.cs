namespace LibrarySystemWebAPI.Entities
{
    public class Borrower : BaseEntity
    {
        public string Name { get; set; }

        public DateTime MembershipDate { get; set; }

        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
