using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Common;
using Microsoft.AspNetCore.Identity;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Services
{
    public class ResetPassword : IResetPassword
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPassword(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult<Guid>> HardResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<Guid>.Failure(new List<Error> { new Error { Message = "User not found.", Code = "UserNotFound" } });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return Result<Guid>.Success(Guid.NewGuid());
            }

            var errors = result.Errors.Select(e => new Error { Message = e.Description, Code = e.Code }).ToList();
            return Result<Guid>.Failure(errors);
        }
    }
}
