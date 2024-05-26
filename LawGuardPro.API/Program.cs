
using LawGuardPro.Infrastructure;
using LawGuardPro.Infrastructure.Identity;
using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LawGuardPro.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
namespace LawGuardPro.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            var configuration = builder.Configuration;
            DependencyInjection_infra.Register(builder.Services, builder.Environment, configuration);
            var app = builder.Build();

         
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
