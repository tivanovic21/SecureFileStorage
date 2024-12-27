using SecureFileStorage.Core.Dtos;

namespace SecureFileStorage.Core.Interfaces 
{
    public interface IFileService 
    {
        Task<IEnumerable<FileDto>> GetFilesForUserAsync(int userId);
        Task<FileDto> GetFileAsync(int fileId);
        Task AddFileAsync(FileDto file);
        Task<FileDto> UpdateFileAsync(FileDto file);
        Task DeleteFileAsync(int fileId);
        Task<(string signature, string publicKey)> GenerateSignatureAsync(string fileName, int userId, Stream fileStream, DateTime uploadTimestamp);
    }
}