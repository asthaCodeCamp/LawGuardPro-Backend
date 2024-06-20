using LawGuardPro.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string[] permittedExtensions = { ".jpg", ".png" , ".pdf" };
    private readonly string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/temp");

    public FileService()
    {
        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }
    }

    public async Task<bool> UploadFileChunkAsync(IFormFile chunk, string fileName, int chunkIndex, int totalChunks)
    {
        if (chunk == null || chunk.Length == 0)
            throw new ArgumentException("File chunk is not valid.");

        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            throw new ArgumentException("Invalid file type.");
        }

        var tempFilePath = Path.Combine(tempDir, $"{fileName}.part{chunkIndex}");

        using (var stream = new FileStream(tempFilePath, FileMode.Create))
        {
            await chunk.CopyToAsync(stream);
        }

        bool CompleteFileUpload = false;
        if (chunkIndex == totalChunks - 1)
        {
            await CompleteFileUploadAsync(fileName);
            CompleteFileUpload = true;
        }
        return CompleteFileUpload;
    }

    public async Task<string> CompleteFileUploadAsync(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            throw new ArgumentException("Invalid file type.");
        }

        var finalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);

        var tempFiles = Directory.GetFiles(tempDir, $"{fileName}.part*").OrderBy(f => f).ToList();

        using (var finalStream = new FileStream(finalPath, FileMode.Create))
        {
            foreach (var tempFile in tempFiles)
            {
                using (var tempStream = new FileStream(tempFile, FileMode.Open))
                {
                    await tempStream.CopyToAsync(finalStream);
                }

                File.Delete(tempFile);
            }
        }

        return fileName;
    }
}
