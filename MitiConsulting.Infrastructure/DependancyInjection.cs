using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MitiConsulting.Infrastructure.Data;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.Infrastructure.Repository;

namespace MitiConsulting.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // DbContext SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repository
            services.AddScoped<IRapportRepository, RapportRepository>();

            return services;
        }
    }
}
