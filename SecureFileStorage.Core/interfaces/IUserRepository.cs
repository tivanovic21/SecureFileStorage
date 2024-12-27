namespace SecureFileStorage.Core.Interfaces
{
    using SecureFileStorage.Core.Entities;
    using System.Threading.Tasks;
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
        Task<bool> UserIsAdminAsync(int id);
    }
}