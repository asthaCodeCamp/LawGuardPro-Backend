using AutoMapper;
using LawGuardPro.Application.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using LawGuardPro.Application.Common;
using System.Net;
using Microsoft.AspNetCore.Http;
using LawGuardPro.Application.Features.Identity.Interfaces;




namespace LawGuardPro.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private string secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public IdentityService(
        IMapper mapper,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper;
        secretKey = configuration.GetValue<string>("Jwt:Key")!;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<bool> IsUniqueUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null ? true : false;
    }

    public async Task<Result<UserDTO>> Register(RegistrationRequestDTO registrationRequestDTO)
    {
        var user = new ApplicationUser
        {
            UserName = registrationRequestDTO.Email,
            FirstName = registrationRequestDTO.FirstName,
            LastName = registrationRequestDTO.LastName,
            Email = registrationRequestDTO.Email,
            NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
            PhoneNumber = registrationRequestDTO.PhoneNumber,
            CountryResidency = registrationRequestDTO.CountryResidency
        };

        var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
       
        if (!result.Succeeded)
        {

            var errors = result.Errors.Select(error => new Error { Message = error.Description, Code = error.Code }).ToList();
            return Result<UserDTO>.Failure( errors);
        }

        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new IdentityRole("user"));
            await _userManager.AddToRoleAsync(user, "user");
        }
        await _userManager.AddToRoleAsync(user, "user");
        var userDto = _mapper.Map<UserDTO>(user);
        return Result<UserDTO>.Success(userDto);
    }


    public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDTO.UserName);

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        if (user == null || isValid == false)
        {
            
            return Result<LoginResponseDTO>.Failure(new List<Error> { new Error() { Message = "Invalid username or password", Code = "InvalidCredentials" } });
        }

        var roles = await _userManager.GetRolesAsync(user);
        //if the user is found generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);//convert the secretKey from string to bytes
        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Subject = new ClaimsIdentity(new Claim[] {
                   new Claim( ClaimTypes.Name, user.Id.ToString()),
                   new Claim(ClaimTypes.Role, roles.FirstOrDefault())  

                }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //var abc = _mapper.Map<UserDTO>(user);


        LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),//serialized  the token 
            User = _mapper.Map<UserDTO>(user),
            Role = roles.FirstOrDefault()
        };

        return Result<LoginResponseDTO>.Success(loginResponseDTO);
    }

    public async Task<Result<UserDTO>> UpdateUserInfo(UserUpdateDTO userUpdateDto)
    {
        if (userUpdateDto == null)
        {
            return Result<UserDTO>.Failure(new List<Error> { new Error() { Message = "Invalid user data.", Code = "" } });
           
        }

        var user = await _userManager.FindByEmailAsync(userUpdateDto.Email);

        if (user == null)
        {
            var errorMessage = $"User with email '{userUpdateDto.Email}' not found.";
            return Result<UserDTO>.Failure(new List<Error> { new Error() { Message = errorMessage, Code = "UserNotFound" } });
        }

        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.PhoneNumber = userUpdateDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) {
            var errors = result.Errors.Select(error => new Error { Message = error.Description, Code = error.Code }).ToList();
            return Result<UserDTO>.Failure(errors);
        }

        var userDto = _mapper.Map<UserDTO>(user);
        return Result<UserDTO>.Success(userDto);
    }

}
