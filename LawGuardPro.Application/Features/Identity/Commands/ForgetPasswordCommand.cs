using AutoMapper;
using FluentValidation.Validators;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LawGuardPro.Application.Features.Identity.Commands;

public class ForgetPasswordCommand: IRequest<Result>
{
    public string Email { get; set; } = string.Empty;
}

public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result> {
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly IOtpService _otpService;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    //private readonly ISmtpSettings _smtpSettings;
    public ForgetPasswordCommandHandler(IEmailService emailService,
        IMapper mapper,
        IIdentityService IdentityService, 
        IOtpService otpService,
        IConfiguration configuration)  
    {
        _emailService = emailService;
        _mapper = mapper;
        _otpService = otpService;
        _identityService = IdentityService;
        _configuration = configuration;

    }

    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guid? userId = await _identityService.GetUserIdByEmailAsync(request.Email);
            if (userId == null)
            {
                return Result.Failure(StatusCodes.Status404NotFound, new List<Error> { new Error() { Code = "UserIdNotFoundError", Message = "User ID not found." } });
            }
            var otp = await _otpService.GenerateAndSaveTotp(request.Email, userId.ToString()!);
            //var requestScheme = _httpContextAccessor?.HttpContext?.Request.Scheme;
            //var requestHost = _httpContextAccessor?.HttpContext?.Request.Host;
            //string baseUrl = $"{requestScheme}://{requestHost}";
            string frontendBaseUrl = _configuration["AppSettings:FrontendBaseUrl"]!;
            string resetPasswordLink = $"{frontendBaseUrl}/reset-password/{userId}/{otp}";

            EmailMetaData emailMetaData = new EmailMetaData()
            {
                FromName = "LawGuardPro",
                FromEmail = "LawGuardPro@gmail.com",
                ToEmail = request.Email,
                ToName = "LawGuardProUser",
                Subject = "Password Reset Link Inside",
                Body = $"{resetPasswordLink}"
            };
            await _emailService.AddEmailToQueueAsync(emailMetaData);
            return Result.Success(StatusCodes.Status202Accepted);
        }
        catch (Exception ex)
        {
            return Result.Failure(StatusCodes.Status400BadRequest, new List<Error> { new Error() { Code = "EmailServiceError", Message = ex.Message } });
        }
    }
}

