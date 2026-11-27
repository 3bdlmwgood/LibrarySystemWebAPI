namespace LibrarySystemWebAPI.Dtos.AccountDtos
{
    public sealed record UserRegisterRequest(
        string FullName,
        string Email,
        string Password);
}