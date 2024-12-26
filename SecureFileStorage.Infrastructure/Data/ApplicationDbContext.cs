using Microsoft.EntityFrameworkCore;
using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User>? User { get; set; }
    public DbSet<Core.Entities.File>? File { get; set; }
    public DbSet<Core.Entities.FileAccess>? FileAccess { get; set; }
    public DbSet<ActivityLog>? ActivityLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.FileAccess>()
            .HasKey(fa => new { fa.FileId, fa.UserId });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}