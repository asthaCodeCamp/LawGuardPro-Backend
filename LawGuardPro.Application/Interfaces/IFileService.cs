using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Interfaces;

public interface IFileService
{
    Task<bool> UploadFileChunkAsync(IFormFile chunk, string fileName, int chunkIndex, int TotalChunks);
    Task<string> CompleteFileUploadAsync(string fileName);
}
