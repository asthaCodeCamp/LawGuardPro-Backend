using LawGuardPro.Application.Common;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.Application.Features.Identity.Commands;

public class ResetForgottenPasswordCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ResetForgottenPasswordCommandHandler : IRequestHandler<ResetForgottenPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IIdentityService _identityService;

    public ResetForgottenPasswordCommandHandler(UserManager<ApplicationUser> userManager, IOtpService otpService, IIdentityService identityService)
    {
        _userManager = userManager;
        _otpService = otpService;
        _identityService = identityService;
    }

    public async Task<Result> Handle(ResetForgottenPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Failure(StatusCodes.Status404NotFound, new List<Error> { new Error { Code = "UserNotFoundError", Message = "User not found." } });
        }

        var isValidOtp = await _otpService.ValidateAndMarkAsUsed(user.Email!, request.Otp, validityInMinutes: 15);
        if (!isValidOtp)
        {
            return Result.Failure(StatusCodes.Status400BadRequest, new List<Error> { new Error { Code = "InvalidOtpError", Message = "Invalid or expired OTP." } });
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

        if (result.Succeeded)
        {
            return Result.Success(StatusCodes.Status200OK);
        }

        return Result.Failure(StatusCodes.Status400BadRequest, result.Errors.Select(e => new Error { Code = e.Code, Message = e.Description }).ToList());
    }
}