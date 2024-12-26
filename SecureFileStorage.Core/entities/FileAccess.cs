namespace SecureFileStorage.Core.Entities;

public class FileAccess
{
    public int FileId { get; set; }
    public int UserId { get; set; }
    public string AccessLevel { get; set; } = null!;
    public DateTime GrantedAt { get; set; }

    public virtual File File { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}