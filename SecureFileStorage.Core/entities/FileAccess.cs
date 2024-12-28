using System.ComponentModel.DataAnnotations.Schema;

namespace SecureFileStorage.Core.Entities;

public class FileAccess
{
    public int FileId { get; set; }
    
    [NotMapped]
    public int? UserId { get; set; }
    public required string UserEmail {get; set;}
    public string AccessLevel { get; set; } = null!;
    public DateTime GrantedAt { get; set; }

    public virtual File File { get; set; } = null!;
}