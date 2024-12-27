using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Core.Dtos
{
    public class ActivityLogDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        public User? User { get; set; } = null;
        public Entities.File? File { get; set; } = null;
    }
}