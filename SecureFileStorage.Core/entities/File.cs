namespace SecureFileStorage.Core.Entities;
public class File
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public string EncryptedUrl { get; set; } = null!;
    public string? PublicKey { get; set; } = null;
    public string? Signature { get; set; } = null;
    public int UploaderId { get; set; }
    public DateTime UploadedAt { get; set; }

    public virtual User Uploader { get; set; } = null!;
    public virtual ICollection<FileAccess> FileAccesses { get; set; } = new List<FileAccess>();
    public virtual ICollection<ActivityLog> Activities { get; set; } = new List<ActivityLog>();
}