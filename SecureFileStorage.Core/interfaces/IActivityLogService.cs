using SecureFileStorage.Core.Dtos;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IActivityLogService
    {
        Task LogActivity(int userId, int fileId, string message);
        Task<IEnumerable<ActivityLogDto>> GetActivityLogsForFile(int fileId);
        Task<IEnumerable<ActivityLogDto>> GetAllActivityLogs();
    }
}