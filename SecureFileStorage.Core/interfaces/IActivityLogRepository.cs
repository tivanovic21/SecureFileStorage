using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IActivityLogRepository
    {
        Task AddActivityLogAsync(ActivityLog activityLog);
        Task<IEnumerable<ActivityLog>> GetActivityLogsForFileAsync(int fileId);  
    }
}