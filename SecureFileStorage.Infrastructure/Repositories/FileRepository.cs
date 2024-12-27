using Microsoft.EntityFrameworkCore;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Infrastructure.Data;
using File = SecureFileStorage.Core.Entities.File;

namespace SecureFileStorage.Infrastructure.Repositories 
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<File> AddFileAsync(File file)
        {
            await _dbContext.File!.AddAsync(file);
            await _dbContext.SaveChangesAsync();
            return file;
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _dbContext.File!.FindAsync(fileId);
            if (file != null) {
                _dbContext.File!.Remove(file);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<File> GetFileAsync(int fileId)
        {
            return await _dbContext.File!.FindAsync(fileId);
        }

        public async Task<IEnumerable<File>> GetFilesForUserAsync(int userId)
        {
            return await _dbContext.File!.Include(f => f.Uploader).Where(f => f.UploaderId == userId).ToListAsync();
        }

        public async Task<File> UpdateFileAsync(File file)
        {
            var existingFile = await _dbContext.File!.FindAsync(file.Id);
            if (existingFile == null) throw new KeyNotFoundException("File not found");

            _dbContext.Entry(existingFile).CurrentValues.SetValues(file);
            await _dbContext.SaveChangesAsync();
            return existingFile;
        }
    }
}