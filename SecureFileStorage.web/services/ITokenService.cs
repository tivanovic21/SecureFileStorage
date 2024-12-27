namespace SecureFileStorage.Web.Services
{
    public interface ITokenService
    {
        string GenerateToken(int userId);
        Task<int> GetLoggedInUserIdAsync();
    }
}