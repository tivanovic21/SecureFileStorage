using System.Security.Cryptography;
using System.Text;
using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Interfaces;
using File = SecureFileStorage.Core.Entities.File;

namespace SecureFileStorage.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileStorageService _fileStorageService;

        public FileService(IFileRepository fileRepository, IFileStorageService fileStorageService)
        {
            _fileRepository = fileRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<FileDto> AddFileAsync(FileDto file)
        {
            return MapFileToDto(await _fileRepository.AddFileAsync(MapDtoToFile(file)));
        }

        public async Task DeleteFileAsync(int fileId)
        {
            await _fileRepository.DeleteFileAsync(fileId);
        }

        public async Task<(string signature, string publicKey)> GenerateSignatureAsync(string fileName, int userId, Stream fileStream, DateTime uploadTimestamp)
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            try {
                var privateKey = rsa.ExportParameters(true);
                var publicKey = rsa.ExportParameters(false);

                var publicKeyString = Convert.ToBase64String(rsa.ExportRSAPublicKey());

                fileStream.Position = 0;
                using var sha256 = SHA256.Create();
                var fileHash = await ComputeHashAsync(fileStream, sha256);
                var fileHashString = Convert.ToBase64String(fileHash);
                Console.WriteLine($"Upload file hash: {fileHashString}");
                fileStream.Position = 0;

                var dataToSign = Encoding.UTF8.GetBytes($"{fileName}{userId}{fileHashString}{uploadTimestamp:O}");
                var signedData = rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                var signature = Convert.ToBase64String(signedData);
                return (signature, publicKeyString);
            } finally {
                rsa.PersistKeyInCsp = false;
            }
        }

        public async Task<bool> ValidateDigitalSignature(int fileId)
        {
            var file = await _fileRepository.GetFileAsync(fileId);
            if (file == null || string.IsNullOrEmpty(file.Signature) || string.IsNullOrEmpty(file.PublicKey)) {
                throw new Exception("Dokument ne sadrži valjane podatke za provjeru digitalnog potpisa!");
            }

            var fileStream = await _fileStorageService.GetFileStream(file.EncryptedUrl);


            using var sha256 = SHA256.Create();
            var fileHash = await ComputeHashAsync(fileStream, sha256);
            var fileHashString = Convert.ToBase64String(fileHash);

            var dataToVerify = Encoding.UTF8.GetBytes($"{file.FileName}{file.UploaderId}{fileHashString}{file.UploadedAt:O}");

            var signature = Convert.FromBase64String(file.Signature);
            var publicKey = Convert.FromBase64String(file.PublicKey);

            Console.WriteLine($"FileStream: {fileStream.Length}");
            Console.WriteLine($"Data to Verify (String): {Encoding.UTF8.GetString(dataToVerify)}");
            Console.WriteLine($"Decoded Signature: {Convert.ToBase64String(signature)}");
            Console.WriteLine($"Decoded PublicKey: {Convert.ToBase64String(publicKey)}");
            Console.WriteLine($"File Hash String: {fileHashString}");
            Console.WriteLine($"UploadedAt: {file.UploadedAt:O}");

            using var rsa = new RSACryptoServiceProvider(2048);
            try
            {
                rsa.ImportRSAPublicKey(publicKey, out _);

                bool isValid = rsa.VerifyData(dataToVerify, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                return isValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during RSA operation: " + ex.Message);
                return false;
            }
        }

        public async Task<FileDto> GetFileAsync(int fileId)
        {
            return MapFileToDto(await _fileRepository.GetFileAsync(fileId));
        }

        public async Task<IEnumerable<FileDto>> GetFilesForUserAsync(int userId)
        {
            var files = await _fileRepository.GetFilesForUserAsync(userId);
            return files.Select(MapFileToDto).ToList();
        }

        public async Task<FileDto> UpdateFileAsync(FileDto file)
        {
            return MapFileToDto(await _fileRepository.UpdateFileAsync(MapDtoToFile(file)));
        }

        private File MapDtoToFile(FileDto file) {
            return new File {
                Id = file.Id,
                FileName = file.FileName,
                EncryptedUrl = file.EncryptedUrl,
                PublicKey = file.PublicKey,
                Signature = file.Signature,
                UploaderId = file.UploaderId,
                UploadedAt = file.UploadedAt
            };
        }

        private FileDto MapFileToDto(File file) {
            return new FileDto {
                Id = file.Id,
                FileName = file.FileName,
                EncryptedUrl = file.EncryptedUrl ?? string.Empty,
                PublicKey = file.PublicKey ?? string.Empty,
                Signature = file.Signature ?? string.Empty,
                UploaderId = file.UploaderId,
                UploadedAt = file.UploadedAt,
                Uploader = file.Uploader
            };
        }

        private async Task<byte[]> ComputeHashAsync(Stream stream, HashAlgorithm hashAlgorithm) {
            var buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
            }
            hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            return hashAlgorithm.Hash;
        }
    }
}