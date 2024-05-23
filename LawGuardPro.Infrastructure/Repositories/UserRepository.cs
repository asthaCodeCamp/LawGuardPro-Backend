using LawGuardPro.Application.DTO;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Domain.Entities;
using AutoMapper;

namespace LawGuardPro.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("Jwt:Key");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public bool IsUniqueUser(string Email)
        {
            // var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email == Email);
            if (user == null)
            {
                return true;
            }
            return false;
        }
        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                FirstName = registrationRequestDTO.FirstName,
                LastName = registrationRequestDTO.LastName,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                PhoneNumber = registrationRequestDTO.PhoneNumber,
                CountryResidency = registrationRequestDTO.CountryResidency,

            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("user").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                        //await _roleManager.CreateAsync(new IdentityRole("admin"));
                    }
                    await _userManager.AddToRoleAsync(user, "user");
                    return _mapper.Map<UserDTO>(user);
                }
            }
            catch (Exception e)
            {

            }
            return new UserDTO();
        }
    }
}
