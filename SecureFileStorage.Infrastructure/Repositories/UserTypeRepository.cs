using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SecureFileStorage.Infrastructure.Repositories
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTypeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<UserType>> GetUserTypesAsync()
        {
            return await _dbContext.UserType!.ToListAsync();
        }
    }
}