using MediatR;
using LawGuardPro.Application.DTO;
using Microsoft.Extensions.Options;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Application.Services;
using LawGuardPro.Application.Settings;

namespace LawGuardPro.Application.Features.Settings.Support.Commands;

public class SupportEmailCommand : IRequest<IResult>
{
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IList<EmailAttachmentDto>? Attachments { get; set; }
}

public class SupportMailCommandHandler : IRequestHandler<SupportEmailCommand, IResult>
{
    private readonly IEmailService _service;
    private readonly IUserContext _userContext;
    private readonly AppSettings _appSettings;


    public SupportMailCommandHandler(IEmailService service,
                                     IUserContext userContext,
                                     IOptions<AppSettings> appSettings)
    {
        _service = service;
        _userContext = userContext;
        _appSettings = appSettings.Value;
    }

    public async Task<IResult> Handle(SupportEmailCommand request, CancellationToken ct)
    {
        var userEmail = _userContext.Email;

        if (string.IsNullOrEmpty(userEmail)) return Result.Failure(new Error
        {
            Message = "User email not found",
            Code = "UserEmail.NotFound"
        });

        var isSent = await _service.SendEmailAsync(new EmailMetaData
        {
            FromEmail = _appSettings.FromEmail,
            FromName = _appSettings.FromName,
            ToEmail = userEmail,
            Subject = request.Subject,
            Body = request.Description!,
            Attachements = request.Attachments!
        });

        return isSent ? Result.Success() : Result.Failure(new Error
        {
            Message = "Email sent failed.",
            Code = "SupportEmail.Failed"
        });
    }
}