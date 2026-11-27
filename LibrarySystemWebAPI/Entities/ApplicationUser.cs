using Microsoft.AspNetCore.Identity;

namespace LibrarySystemWebAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}