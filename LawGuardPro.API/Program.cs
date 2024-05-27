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
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
