using File = SecureFileStorage.Core.Entities.File;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IFileRepository
    {
        Task<File> GetFileAsync(int fileId);
        Task<IEnumerable<File>> GetFilesForUserAsync(int userId);
        Task AddFileAsync(File file);
        Task<File> UpdateFileAsync(File file);
        Task DeleteFileAsync(int fileId);
    }
}