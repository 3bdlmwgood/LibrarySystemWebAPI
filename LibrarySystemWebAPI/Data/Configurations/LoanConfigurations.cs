
using LibrarySystemWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystemWebAPI.Data.Configurations
{
    public class LoanConfigurations
        : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Composite Primary Key
            builder.HasKey(l => new
            {
                l.BookId,
                l.BorrowerId
            });

            builder.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);

            builder.HasOne(l => l.Borrower)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BorrowerId);

            builder.HasQueryFilter(l => !l.IsDeleted);
        }
    }
}
