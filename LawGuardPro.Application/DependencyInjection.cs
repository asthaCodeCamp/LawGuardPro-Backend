using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
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
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
 