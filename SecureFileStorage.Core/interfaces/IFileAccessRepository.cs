using FileAccess = SecureFileStorage.Core.Entities.FileAccess;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IFileAccessRepository
    {
        Task<IEnumerable<FileAccess>> GetFileAccessesForUser(string email, int? userId = null);
        Task<bool> AddFileAccessAsync(FileAccess fileAccess);
    }
}