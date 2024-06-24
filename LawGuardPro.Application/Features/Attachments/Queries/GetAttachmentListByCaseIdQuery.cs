using MediatR;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Features.Attachments.Queries;

public class GetAttachmentListByCaseIdQuery : IRequest<IResult<List<AttachmentDto>>>
{
    public Guid CaseId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAttachmentListByCaseIdQuery(Guid caseId, int pageNumber, int pageSize)
    {
        CaseId = caseId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetAttachmentListByCaseIdQueryHandler : IRequestHandler<GetAttachmentListByCaseIdQuery, IResult<List<AttachmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAttachmentListByCaseIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<List<AttachmentDto>>> Handle(GetAttachmentListByCaseIdQuery request, CancellationToken cancellationToken)
    {
        var attachments = await _unitOfWork.CaseRepository.GetAttachmentsByCaseIdAsync(request.CaseId, request.PageNumber, request.PageSize);

        var attachmentDtos = attachments.Select(a => new AttachmentDto
        {
            AttachmentId = a.AttachmentId,
            Title = a.Title,
            FileURL = a.FileURL,
            Type = a.Type,
            UploadedBy = a.UploadedBy,
            AddedOn = a.AddedOn,
            CaseId = a.CaseId
        }).ToList();

        return Result<List<AttachmentDto>>.Success(attachmentDtos);
    }
}