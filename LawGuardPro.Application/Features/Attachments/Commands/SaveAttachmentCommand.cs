using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.Application.Features.Attachments.Commands;

public class SaveAttachmentCommand : IRequest<IResult<string>>
{
    public Guid CaseId { get; set; }
    public List<string> FileUrls { get; set; }

    public SaveAttachmentCommand(Guid caseId, List<string> fileUrls)
    {
        CaseId = caseId;
        FileUrls = fileUrls;
    }
}

public class SaveAttachmentCommandHandler : IRequestHandler<SaveAttachmentCommand, IResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ICaseRepository _caseRepository;
    private readonly IRepository<Attachment> _attachmentRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public SaveAttachmentCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper, ICaseRepository caseRepository, IRepository<Attachment> attachmentRepository, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _caseRepository = caseRepository;
        _attachmentRepository = attachmentRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IResult<string>> Handle(SaveAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "User not found", Code = "Not Found" } });
            }

            var caseEntity = await _caseRepository.GetByIdAsync(request.CaseId);
            if (caseEntity == null)
            {
                return Result<string>.Failure(new List<Error> { new Error { Message = "Case not found", Code = "Not Found" } });
            }

            var attachments = new List<Attachment>();
            for (int i = 0; i < request.FileUrls.Count; i++)
            {
                var (fileName, fileType) = GetFileNameAndType(request.FileUrls[i]);
                
                var attachment = new Attachment
                {
                    AttachmentId = Guid.NewGuid(),
                    CaseId = request.CaseId,
                    Title = fileName,
                    FileURL = request.FileUrls[i],
                    Type = fileType.ToUpper(),
                    UploadedBy = user.FirstName+" "+user.LastName,
                    AddedOn = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                attachments.Add(attachment);
            }

            foreach (var attachment in attachments)
            {
                await _attachmentRepository.AddAsync(attachment);
            }

            await _unitOfWork.CommitAsync();

            return Result<string>.Success("Attachments Saved");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new List<Error> { new Error { Message = "An error occurred while saving attachments: " + ex.Message, Code = "Unknown" } });
        }
    }

    private (string FileName, string FileType) GetFileNameAndType(string fileUrl)
    {
        var uri = new Uri(fileUrl);
        var fileName = System.IO.Path.GetFileName(uri.LocalPath);
        var fileType = System.IO.Path.GetExtension(fileName).TrimStart('.');
        return (fileName, fileType);
    }
}