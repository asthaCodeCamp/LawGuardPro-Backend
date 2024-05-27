using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Features.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> IsUniqueUser(string email);
        Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO);

        Task<Result<UserDTO>> Register(RegistrationRequestDTO registration);
    }
}
