using Application.Handlers;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCleanArchitectureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("StationDBContext"),
                    sqlOptions =>
                        sqlOptions.MigrationsAssembly(
                            typeof(ApplicationDbContext).Assembly.FullName)));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(configuration.GetConnectionString("StationDBContext")
            //?? throw new InvalidOperationException("Connection string 'StationDBContext' not found.")));

            // Repositories
            services.AddScoped<IStationRepository, StationRepository>();

            // MediatR
            services.AddMediatR(typeof(CreateStationCommandHandler).Assembly);

            return services;
        }
    }
}