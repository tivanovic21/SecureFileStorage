using SecureFileStorage.Core.Entities;
using File = SecureFileStorage.Core.Entities.File;

namespace SecureFileStorage.Core.Dtos
{
    public class FileAccessDto
    {
        public int FileId { get; set; }
        public int? UserId { get; set; }
        public required string UserEmail { get; set; }
        public required string AccessLevel { get; set; }
        public DateTime GrantedAt { get; set; }

        public File? File { get; set; }
    }
}