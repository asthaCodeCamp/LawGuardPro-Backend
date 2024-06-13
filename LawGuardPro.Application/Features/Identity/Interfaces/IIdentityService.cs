using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using System.Collections.Generic;

namespace LawGuardPro.Application.Features.Identity.Interfaces;

public interface IIdentityService{
        Task<bool> IsUniqueUser(string email);

        Task<IResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<IResult<UserDTO>> RegisterAsync(RegistrationRequestDTO registration);

        Task<IResult<UserDTO>> UpdateUserInfoAsync(UserUpdateDTO userUpdateDto);
 }

