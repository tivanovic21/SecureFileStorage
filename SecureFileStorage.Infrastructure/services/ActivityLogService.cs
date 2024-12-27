using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SecureFileStorage.Infrastructure.Services
{
    public class ActivityLogService : IActivityLogService 
    {
        private readonly IActivityLogRepository _activityLogRepository;
        public ActivityLogService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        public async Task<IEnumerable<ActivityLogDto>> GetActivityLogsForFile(int fileId)
        {
            var logs = await _activityLogRepository.GetActivityLogsForFileAsync(fileId);
            return logs.Select(MapActivityLogToDto).ToList();
        }

        public async Task LogActivity(int userId, int fileId, string message)
        {
            var activityLog = new ActivityLog
            {
                UserId = userId,
                FileId = fileId,
                Action = message,
                Timestamp = DateTime.UtcNow
            };
            await _activityLogRepository.AddActivityLogAsync(activityLog);
        }

        private ActivityLogDto MapActivityLogToDto(ActivityLog activityLog)
        {
            return new ActivityLogDto
            {
                Id = activityLog.Id,
                UserId = activityLog.UserId,
                FileId = activityLog.FileId,
                Action = activityLog.Action,
                Timestamp = activityLog.Timestamp,
                User = activityLog.User ?? null,
            };
        }
    }
}