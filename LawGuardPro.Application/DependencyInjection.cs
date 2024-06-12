using System.Reflection;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using LawGuardPro.Application.Common.Mappings;

namespace LawGuardPro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(option =>
            option.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}