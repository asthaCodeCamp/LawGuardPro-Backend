using System.Text;
using LawGuardPro.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LawGuardPro.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using LawGuardPro.Infrastructure.Identity;
using LawGuardPro.Infrastructure.Services;
using LawGuardPro.Infrastructure.Settings;
using LawGuardPro.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LawGuardPro.Infrastructure.Persistence.Context;
using LawGuardPro.Application.Features.Identity.Interfaces;
using LawGuardPro.Application.Services;
using LawGuardPro.Infrastructure.UnitofWork;

namespace LawGuardPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseNpgsql(configuration.GetConnectionString("DefaultSQLConnection")));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAddressRepository, AddressRepository>();

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddAutoMapper(typeof(MappingConfig));

        var key = configuration.GetValue<string>("Jwt:Key");
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddTransient<IEmailRepository, EmailRepository>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICaseRepository, CaseRepository>();
        services.AddScoped<ILawyerRepository, LawyerRepository>();
        services.AddScoped<IOtpService, OtpService>();

        return services;
    }
}
