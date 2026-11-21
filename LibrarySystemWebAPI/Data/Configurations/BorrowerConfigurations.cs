
using LibrarySystemWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystemWebAPI.Data.Configurations
{
    public class BorrowerConfigurations
        : IEntityTypeConfiguration<Borrower>
    {
        public void Configure(EntityTypeBuilder<Borrower> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
