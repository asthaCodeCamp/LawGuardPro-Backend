using LawGuardPro.Api;
using LawGuardPro.Application;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure;
using LawGuardPro.Infrastructure.Repositories;
using LawGuardPro.Infrastructure.Services;
using LawGuardPro.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace LawGuardPro.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var configuration = builder.Configuration;
        object value = builder.Services
                                .AddApi()
                                .AddApplication()
                                .AddInfrastructure(configuration)
                                .AddScoped<IUnitOfWork, UnitOfWork>()
                                .AddAutoMapper(typeof(Program))
                                .AddScoped<ICaseRepository, CaseRepository>()
                                .AddScoped<ILawyerRepository, LawyerRepository>()
                                .AddControllers();

        builder.Services.AddHostedService<EmailSenderService>();
        var app = builder.Build();
        app.UseApi();
        
        app.MapControllers();
        app.Run();
    }
}
