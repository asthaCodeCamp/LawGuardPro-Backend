using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Application.Features.Files.Commands;

public class CompleteFileUploadCommand : IRequest<IResult<string>>
{
    public string FileName { get; set; }

    public CompleteFileUploadCommand(string fileName)
    {
        FileName = fileName;
    }
}

public class CompleteFileUploadCommandHandler : IRequestHandler<CompleteFileUploadCommand, IResult<string>>
{
    private readonly IFileService _fileService;

    public CompleteFileUploadCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IResult<string>> Handle(CompleteFileUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return Result<string>.Success(await _fileService.CompleteFileUploadAsync(request.FileName));
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new List<Error> { new Error() { Code = "EmailServiceError", Message = ex.Message } });
        }
    }
}