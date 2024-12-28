namespace SecureFileStorage.Core.Entities;
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public int UserTypeId {get; set;}
    
    public virtual ICollection<File> UploadedFiles { get; set; } = new List<File>();
    public virtual ICollection<ActivityLog> Activities { get; set; } = new List<ActivityLog>();
    public virtual UserType UserType { get; set; } = null!;
}