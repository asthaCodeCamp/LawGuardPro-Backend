using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LawGuardPro.Application.Features.Identity.Commands;

public class SendEmailCommand : IRequest<Result>
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string? Body { get; set; }
}

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result>
{
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    public SendEmailCommandHandler(IEmailService emailService, IMapper mapper)
    {
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _emailService.SendEmailAsync(_mapper.Map<EmailMetaData>(request));
            return Result.Success(StatusCodes.Status202Accepted);
        }
        catch (Exception ex)
        {
            return Result.Failure(StatusCodes.Status400BadRequest, new List<Error> { new Error() { Code= "EmailServiceError", Message=ex.Message } });
        }
    }
}

