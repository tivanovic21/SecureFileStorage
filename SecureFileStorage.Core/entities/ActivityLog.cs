namespace SecureFileStorage.Core.Entities;

public class ActivityLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FileId { get; set; }
    public string Action { get; set; } = null!;
    public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual File File { get; set; } = null!;
}