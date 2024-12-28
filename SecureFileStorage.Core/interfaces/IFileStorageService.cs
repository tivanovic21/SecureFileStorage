namespace SecureFileStorage.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, int userId);
        string DecryptUrl(string encryptedUrl);
        Task<(Stream FileStream, string FileName)> DownloadFileAsync(string encryptedUrl);
        Task<Stream> GetFileStream(string encryptedUrl);
    }
}