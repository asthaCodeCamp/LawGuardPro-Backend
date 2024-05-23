using LawGuardPro.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Identity
{
    public interface IIdentityService
    {
        bool IsUniqueUser(string email);
        //Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UserDTO> Register(RegistrationRequestDTO registration);
    }
}
