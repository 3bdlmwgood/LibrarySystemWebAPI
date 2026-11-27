using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Data
{
    public class AuthenticationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options): base(options)
        {
        }
    }
}