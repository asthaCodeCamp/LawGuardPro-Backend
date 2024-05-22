using LawGuardPro.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawGuardPro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace LawGuardPro.Infrastructure
{
    public class DependencyInjection_infra
    {
        public static IServiceCollection Register(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseNpgsql(configuration.GetConnectionString("EfPostgresDb")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}
