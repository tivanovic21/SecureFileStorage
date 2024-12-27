namespace SecureFileStorage.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, int userId);
        string DecryptUrl(string encryptedUrl);
    }
}