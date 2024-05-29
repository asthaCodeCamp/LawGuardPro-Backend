using LawGuardPro.Api;
using LawGuardPro.Application;
using LawGuardPro.Infrastructure;

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
                                .AddInfrastructure(configuration);

        var app = builder.Build();
        app.UseApi();
        
        app.MapControllers();
        app.Run();
    }
}
