using SecureFileStorage.Core.Dtos;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IFileAccessService
    {
        Task<IEnumerable<FileAccessDto>> GetFileAccessesForUser(string email, int? id = null);
        Task<bool> AddFileAccessAsync(int fileId, string userEmail, int? userId = null);
    }
}