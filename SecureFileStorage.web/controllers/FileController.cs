using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SecureFileStorage.Core.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    public FilesController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile(string encryptedUrl)
    {
        try {
            var (fileStream, fileName) = await _fileStorageService.DownloadFileAsync(encryptedUrl);
            var contentType = GetContentType(fileName);
            return File(fileStream, contentType, fileName);
        } catch (FileNotFoundException) {
            return NotFound("Requested file not found!");
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private string GetContentType(string fileName) {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType)) {
            contentType = "application/octet-stream";
        }
        return contentType;
    }        
}