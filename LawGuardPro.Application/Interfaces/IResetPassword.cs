using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Interfaces;

public interface IResetPassword
{
   Task<Result<Guid>> HardResetPasswordAsync(string email, string newPassword);
}
