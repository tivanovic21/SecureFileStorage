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

        modelBuilder.Entity<Core.Entities.FileAccess>(entity =>
        {
            entity.HasKey(fa => new { fa.FileId, fa.UserEmail });

            entity.HasOne(fa => fa.File)
                .WithMany(f => f.FileAccesses)
                .HasForeignKey(fa => fa.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(fa => fa.UserId)
                .HasColumnName("UserId") 
                .IsRequired(false); 
        });

        modelBuilder.Entity<UserType>().HasData(
            new UserType { Id = 1, Name = "Admin" },
            new UserType { Id = 2, Name = "Korisnik" }
        );
    }
}