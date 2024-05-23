using LawGuardPro.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        //Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UserDTO> Register(RegistrationRequestDTO registration);
    }
}
