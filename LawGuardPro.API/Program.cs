using LawGuardPro.Api;
using LawGuardPro.Application;
using LawGuardPro.Infrastructure;
using LawGuardPro.Application.Interfaces;

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
            .AddInfrastructure(configuration);
        builder.Services.AddHostedService<EmailSenderService>();

        var app = builder.Build();
        app.UseApi();

        app.MapControllers();
        app.Run();
    }
}