using Microsoft.EntityFrameworkCore;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Infrastructure.Data;

namespace SecureFileStorage.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivityLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddActivityLogAsync(ActivityLog activityLog)
        {
            await _dbContext.ActivityLog!.AddAsync(activityLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetActivityLogsForFileAsync(int fileId)
        {
            return await _dbContext.ActivityLog!
                .Include(a => a.User)
                .Include(a => a.File)
                .Where(a => a.FileId == fileId)
                .ToListAsync();
        }
    }
}