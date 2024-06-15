using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using System.Collections.Generic;

namespace LawGuardPro.Application.Features.Identity.Interfaces;

public interface IIdentityService{
        Task<bool> IsUniqueUser(string email);
        Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<Result<UserDTO>> RegisterAsync(RegistrationRequestDTO registration);
        Task<Result<UserUpdateDTO>> UpdateUserInfoAsync(UserUpdateDTO userUpdateDto);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<string?> GetEmailByUserIdAsync(string userId);
 }

