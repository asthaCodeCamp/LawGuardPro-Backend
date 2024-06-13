using LawGuardPro.Application.Common;

namespace LawGuardPro.Application.Interfaces;

public interface IResetPassword
{
   Task<IResult<Guid>> HardResetPasswordAsync(string email, string newPassword);
}
