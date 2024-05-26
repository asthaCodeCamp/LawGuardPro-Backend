using AutoMapper;
using LawGuardPro.Application.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using LawGuardPro.Application.Feature.Identity.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;



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

    public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDTO.Email,
            FirstName = registrationRequestDTO.FirstName,
            LastName = registrationRequestDTO.LastName,
            Email = registrationRequestDTO.Email,
            NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
            PhoneNumber = registrationRequestDTO.PhoneNumber,
            CountryResidency = registrationRequestDTO.CountryResidency,
        };

        var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
        if (!result.Succeeded) throw new Exception();

        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new IdentityRole("user"));
            await _userManager.AddToRoleAsync(user, "user");
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDTO.UserName);

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }

        //var roles = await _userManager.GetRolesAsync(user);
        //if the user is found generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);//convert the secretKey from string to bytes
        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Subject = new ClaimsIdentity(new Claim[] {
                   new Claim( ClaimTypes.Name, user.Id.ToString()),
                   new Claim(ClaimTypes.Role, "user")  // roles.FirstOrDefault()

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
            Role = "user" // roles.FirstOrDefault()
        };

        return loginResponseDTO;
    }

}
