using Microsoft.EntityFrameworkCore;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IDbContext
    {
        DbSet<Entities.File>? File {get; set;}
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}