using LawGuardPro.Application.Common;
using LawGuardPro.Application.Features.Attachments.Commands;
using LawGuardPro.Application.Features.Attachments.Queries;
using LawGuardPro.Application.Features.Files.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LawGuardPro.API.Controllers;

[Route("api/filecontroller")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload-chunk")]
    public async Task<IActionResult> UploadFileChunk( IFormFile chunk, [FromForm] string fileName, [FromForm] int chunkIndex, [FromForm] int totalChunks)
    {
        try
        {

            var command = new UploadFileChunkCommand(chunk, fileName, chunkIndex, totalChunks);
            
            IResult<bool> result = await _mediator.Send(command);
            if (result.IsSuccess() && result.Data == false) {
                return Ok(new { Message = "Chunk uploaded successfully." });
            }
            else if(result.IsSuccess() && result.Data == true)
            {
                var fileUrl = $"{Request.Scheme}://{Request.Host}/files/{fileName}";
                return Ok(new { fileUrl });
            }
            else
            {
                return BadRequest();
            }
            
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    //[HttpPost("complete-upload")]
    //public async Task<IActionResult> CompleteFileUpload([FromForm] string fileName)
    //{
    //    try
    //    {
    //        var command = new CompleteFileUploadCommand(fileName);
    //        var finalFileName = (await _mediator.Send(command)).Data;
    //        var fileUrl = $"{Request.Scheme}://{Request.Host}/files/{finalFileName}";
    //        return Ok(new { fileUrl });
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    //    }
    //}

    [HttpPost("save-attachments")]
    public async Task<IActionResult> SaveAttachments([FromBody] SaveAttachmentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("attachments")]
    public async Task<IActionResult> GetAttachmentsByCaseId([FromQuery] Guid caseId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var query = new GetAttachmentListByCaseIdQuery(caseId, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }
}