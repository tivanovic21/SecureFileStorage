using Microsoft.EntityFrameworkCore;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;

namespace SecureFileStorage.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User>? User { get; set; }
    public DbSet<UserType>? UserType { get; set; }
    public DbSet<Core.Entities.File>? File { get; set; }
    public DbSet<Core.Entities.FileAccess>? FileAccess { get; set; }
    public DbSet<ActivityLog>? ActivityLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Core.Entities.FileAccess>()
            .HasKey(fa => new { fa.FileId, fa.UserEmail });

        modelBuilder.Entity<Core.Entities.FileAccess>()
            .HasOne(fa => fa.User)
            .WithMany(u => u.FileAccesses)
            .HasPrincipalKey(u => new { u.Id, u.Email })
            .HasForeignKey(fa => new { fa.UserId, fa.UserEmail })
            .IsRequired(false);

        modelBuilder.Entity<UserType>().HasData(
            new UserType { Id = 1, Name = "Admin" },
            new UserType { Id = 2, Name = "Korisnik" }
        );
    }
}