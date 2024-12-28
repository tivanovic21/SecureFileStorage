using System.Security.Cryptography;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using SecureFileStorage.Core.Interfaces;

namespace SecureFileStorage.Infrastructure.Services
{
    public class AzureBlobService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly IConfiguration _configuration;

        public AzureBlobService(IConfiguration configuration)
        {
            _configuration = configuration;
            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
            _containerName = configuration["AzureStorage:ContainerName"];
        }

        public string DecryptUrl(string encryptedUrl)
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_configuration["FileEncryption:Key"]);
            aes.IV = Convert.FromBase64String(_configuration["FileEncryption:IV"]);

            using var decryptor = aes.CreateDecryptor();
            var encryptedBytes = Convert.FromBase64String(encryptedUrl);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }

        public async Task<(Stream FileStream, string FileName)> DownloadFileAsync(string encryptedUrl)
        {
            var url = DecryptUrl(encryptedUrl);
            Uri blobUri = new Uri(url);

            string blobName = Uri.UnescapeDataString(blobUri.AbsolutePath.TrimStart('/'));
            if (blobName.StartsWith($"{_containerName}/"))
            {
                blobName = blobName.Substring(_containerName.Length + 1);
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if(!await blobClient.ExistsAsync())
            {
                throw new FileNotFoundException("The requested file not found in Azure Blob Storage!");
            }

            try {
                var properties = await blobClient.GetPropertiesAsync();
                var fileName = properties.Value.Metadata.ContainsKey("FileName") ? properties.Value.Metadata["FileName"] : blobName;
                var downloadResponse = await blobClient.DownloadContentAsync();
                return (new MemoryStream(downloadResponse.Value.Content.ToArray()), fileName);
            } catch (Exception ex) {
                Console.WriteLine("Greška prilikom preuzimanja s Azure-a: " + ex.Message);
                throw;
            }
        }

        public async Task<Stream> GetFileStream(string encryptedUrl)
        {
            var (fileStream, _) = await DownloadFileAsync(encryptedUrl);
            fileStream.Position = 0;
            return fileStream;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, int userId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var encodedFileName = Uri.EscapeDataString(fileName);
            var blobName = $"{userId}/{Guid.NewGuid()}_{encodedFileName}";
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(fileStream, overwrite: true);

            var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
            return EncryptUrl(sasUri.ToString());
        }

        private string EncryptUrl(string url) {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_configuration["FileEncryption:Key"]);
            aes.IV = Convert.FromBase64String(_configuration["FileEncryption:IV"]);

            using var encryptor = aes.CreateEncryptor();
            var urlBytes = System.Text.Encoding.UTF8.GetBytes(url);
            var encryptedBytes = encryptor.TransformFinalBlock(urlBytes, 0, urlBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
    }
}