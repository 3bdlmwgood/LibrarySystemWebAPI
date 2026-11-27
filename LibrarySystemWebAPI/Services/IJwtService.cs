using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}