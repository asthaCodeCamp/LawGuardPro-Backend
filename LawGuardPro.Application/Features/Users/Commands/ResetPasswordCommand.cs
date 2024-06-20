using MediatR;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace LawGuardPro.Application.Features.Users.Commands;

public class ResetPasswordCommand : IRequest<IResult<Guid>>
{
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult<Guid>>
{
    private readonly IResetPassword _resetPassword;
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(IResetPassword resetPassword, IUserContext userContext, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _resetPassword = resetPassword;
        _userContext = userContext;
        _userManager = userManager;
        _passwordHasher = passwordHasher;

    }

    public async Task<IResult<Guid>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(_userContext.Email);
        if (user == null)
        {
            return Result<Guid>.Failure(new List<Error> { new Error { Message = "User not found.", Code = "UserNotFound" } });
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return Result<Guid>.Failure(new List<Error> { new Error { Message = "Password hash is null or empty.", Code = "InvalidPasswordHash" } });
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            return Result<Guid>.Failure(new List<Error> { new Error { Message = "Old password does not match.", Code = "InvalidPassword" } });
        }

        return await _resetPassword.HardResetPasswordAsync(_userContext.Email, request.NewPassword);
    }
}