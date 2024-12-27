namespace SecureFileStorage.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SecureFileStorage.Core.Entities;
    using SecureFileStorage.Core.Interfaces;
    using SecureFileStorage.Infrastructure.Data;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.User!.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.User!.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.User!.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbContext.User!.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserIsAdminAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user?.UserTypeId == 1) return true;

            return false;
        }
    }
}