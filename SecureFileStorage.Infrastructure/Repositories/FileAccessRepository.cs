using Microsoft.EntityFrameworkCore;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Infrastructure.Data;
using FileAccess = SecureFileStorage.Core.Entities.FileAccess;

namespace SecureFileStorage.Infrastructure.Repositories
{
    public class FileAccessRepository : IFileAccessRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FileAccessRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FileAccess>> GetFileAccessesForUser(string email, int? userId = null)
        {
            var query = _dbContext.FileAccess!.AsQueryable();

            query = query.Where(fa => fa.UserEmail == email);

            return await query
                .Include(a => a.File)
                    .ThenInclude(f => f.Uploader)
                .ToListAsync();
        }

        public async Task<bool> AddFileAccessAsync(FileAccess fileAccess)
        {
            var existingAccess = await _dbContext.FileAccess!
                .FirstOrDefaultAsync(fa => fa.FileId == fileAccess.FileId && fa.UserEmail == fileAccess.UserEmail);

            if (existingAccess != null)
            {
                existingAccess.UserId = fileAccess.UserId;
                existingAccess.AccessLevel = fileAccess.AccessLevel;
                existingAccess.GrantedAt = fileAccess.GrantedAt;
            }
            else 
            {
                if (fileAccess.UserId == null)
                {
                    var user = await _dbContext.User!
                        .FirstOrDefaultAsync(u => u.Email == fileAccess.UserEmail);
                    if (user != null) 
                    {
                        fileAccess.UserId = user.Id;
                    }
                    else
                    {
                        fileAccess.UserId = null;
                    }
                }
                await _dbContext.FileAccess!.AddAsync(fileAccess);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}