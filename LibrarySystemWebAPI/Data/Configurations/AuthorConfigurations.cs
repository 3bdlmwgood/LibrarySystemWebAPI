
using LibrarySystemWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystemWebAPI.Data.Configurations
{
    public class AuthorConfigurations
        : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}
