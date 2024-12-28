using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Infrastructure.Enums;
using FileAccess = SecureFileStorage.Core.Entities.FileAccess;

namespace SecureFileStorage.Infrastructure.Services
{
    public class FileAccessService : IFileAccessService
    {
        private readonly IFileAccessRepository _fileAccessRepository;
        public FileAccessService(IFileAccessRepository fileAccessRepository)
        {
            _fileAccessRepository = fileAccessRepository;
        }

        public async Task<bool> AddFileAccessAsync(int fileId, string userEmail, int? userId = null)
        {
            var existingFileAccess = await _fileAccessRepository.GetFileAccessesForUser(userEmail, userId);
            if (existingFileAccess.Any(f => f.FileId == fileId))
            {
                throw new Exception("Korisnik već ima pravo pristupa dokumentu!");
            }

            return await _fileAccessRepository.AddFileAccessAsync(MapDtoToFileAccess(new FileAccessDto {
                FileId = fileId,
                UserEmail = userEmail,
                UserId = userId,
                AccessLevel = ActivityLogEnum.Access,
                GrantedAt = DateTime.Now
            }));
        }

        public async Task<IEnumerable<FileAccessDto>> GetFileAccessesForUser(string email, int? id = null)
        {
            var files = await _fileAccessRepository.GetFileAccessesForUser(email, id);
            return files.Select(MapFileAccessToDto).ToList();
        }

        private FileAccess MapDtoToFileAccess(FileAccessDto fileAccessDto)
        {
            return new FileAccess {
                FileId = fileAccessDto.FileId,
                UserId = fileAccessDto.UserId,
                UserEmail = fileAccessDto.UserEmail,
                AccessLevel = fileAccessDto.AccessLevel,
                GrantedAt = fileAccessDto.GrantedAt
            };
        }

        private FileAccessDto MapFileAccessToDto(FileAccess fileAccess)
        {
            return new FileAccessDto {
                FileId = fileAccess.FileId,
                UserId = fileAccess.UserId,
                UserEmail = fileAccess.UserEmail,
                AccessLevel = fileAccess.AccessLevel,
                GrantedAt = fileAccess.GrantedAt,
                File = fileAccess.File,
                // User = fileAccess.User
            };
        }
    }
}