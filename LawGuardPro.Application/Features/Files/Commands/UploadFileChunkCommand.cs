using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Interfaces;
using MediatR;
using LawGuardPro.Application.Common;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Application.Features.Files.Commands;

public class UploadFileChunkCommand : IRequest<IResult<bool>>
{
    public IFormFile Chunk { get; set; }
    public string FileName { get; set; }
    public int ChunkIndex { get; set; }
    public int TotalChunks { get; set; }

    public UploadFileChunkCommand(IFormFile chunk, string fileName, int chunkIndex, int totalChunks)
    {
        Chunk = chunk;
        FileName = fileName;
        ChunkIndex = chunkIndex;
        TotalChunks = totalChunks;
    }
}
public class UploadFileChunkCommandHandler : IRequestHandler<UploadFileChunkCommand, IResult<bool>>
{
    private readonly IFileService _fileService;

    public UploadFileChunkCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IResult<bool>> Handle(UploadFileChunkCommand request, CancellationToken cancellationToken)
    {
        
        try
        {
            if(await _fileService.UploadFileChunkAsync(request.Chunk, request.FileName, request.ChunkIndex, request.TotalChunks))
            {
                return Result<bool>.Success(true);
            }
            else
            {
                return Result<bool>.Success(false);
            }
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure( new List<Error> { new Error() { Code = "EmailServiceError", Message = ex.Message } });
        }
    }
}