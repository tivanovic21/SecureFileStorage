using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Web.Services
{
    public interface ITokenService
    {
        string GenerateToken(int userId);
        Task<int> GetLoggedInUserIdAsync();
        Task<User?> GetLoggedInUser(int id);
        Task<bool> UserIsAdmin(int id);
    }
}