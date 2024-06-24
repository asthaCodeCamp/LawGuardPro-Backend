using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Features.Attachments.Commands;

public class SaveAttachmentCommand : IRequest<IResult<string>>
{
    public Guid CaseId { get; set; }
    public List<string> FileUrls { get; set; }
    public List<string> Titles { get; set; }
    public List<string> Types { get; set; }

    public SaveAttachmentCommand(Guid caseId, List<string> fileUrls, List<string> titles, List<string> types)
    {
        CaseId = caseId;
        FileUrls = fileUrls;
        Titles = titles;
        Types = types;
    }
}

public class SaveAttachmentCommandHandler : IRequestHandler<SaveAttachmentCommand, IResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public SaveAttachmentCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<IResult<string>> Handle(SaveAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContext.UserId;

            var caseEntity = await _unitOfWork.CaseRepository.GetByIdAsync(request.CaseId);
            if (caseEntity == null)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "Case not found", Code = "Not Found" } });
            }

            for (int i = 0; i < request.FileUrls.Count; i++)
            {
                var attachment = new Attachment
                {
                    AttachmentId = Guid.NewGuid(),
                    CaseId = request.CaseId,
                    Title = request.Titles[i],
                    FileURL = request.FileUrls[i],
                    Type = request.Types[i],
                    UploadedBy = userId.ToString(),
                    AddedOn = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                caseEntity.Attachments.Add(attachment);
            }

            await _unitOfWork.CommitAsync();

            return Result<string>.Success("Attachment Saved");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new List<Error> { new Error { Message = "An error occurred while saving attachments: " + ex.Message, Code = "Not Found" } });

        }
    }
}