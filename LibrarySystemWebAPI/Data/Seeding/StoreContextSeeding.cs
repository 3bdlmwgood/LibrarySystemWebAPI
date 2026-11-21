
using System.Text.Json;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Data
{
    public static class StoreContextSeeding
    {
        public static async Task SeedAsync(ApplicationDBContext dbContext,string contentRootPath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Seed Authors
            if (!dbContext.Authors.Any())
            {
                var path = Path.Combine(
                    contentRootPath,
                    "Data", "Seeding", "Authors.json"
                );

                var json = await File.ReadAllTextAsync(path);

                var authors =JsonSerializer.Deserialize<List<Author>>(json, options);

                if (authors != null)
                {
                    await dbContext.Authors.AddRangeAsync(authors);
                    await dbContext.SaveChangesAsync();
                }
            }

            // Seed Borrowers
            if (!dbContext.Borrowers.Any())
            {
                var path = Path.Combine(
                    contentRootPath,
                    "Data", "Seeding", "Borrowers.json"
                );

                var json = await File.ReadAllTextAsync(path);

                var borrowers =
                    JsonSerializer.Deserialize<List<Borrower>>(json, options);

                if (borrowers != null)
                {
                    await dbContext.Borrowers.AddRangeAsync(borrowers);
                    await dbContext.SaveChangesAsync();
                }
            }

            // Seed Books
            if (!dbContext.Books.Any())
            {
                var path = Path.Combine(
                    contentRootPath,
                    "Data", "Seeding", "Books.json"
                );

                var json = await File.ReadAllTextAsync(path);

                var books =
                    JsonSerializer.Deserialize<List<Book>>(json, options);

                if (books != null)
                {
                    await dbContext.Books.AddRangeAsync(books);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
