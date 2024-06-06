using LawGuardPro.Api;
using LawGuardPro.Application;
using LawGuardPro.Infrastructure;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure.UnitOfWork;
using LawGuardPro.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace LawGuardPro.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;
        builder.Services
            .AddApi()
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddAutoMapper(typeof(Program))
            .AddScoped<ICaseRepository, CaseRepository>()
            .AddScoped<ILawyerRepository, LawyerRepository>()
            .AddControllers();
      
        builder.Services.AddHostedService<EmailSenderService>();      
        builder.Services.AddHttpContextAccessor();
       
        var app = builder.Build();
        app.UseApi();

        app.MapControllers();
        app.Run();
    }
}