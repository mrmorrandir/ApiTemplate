using ApiTemplate.Application.Interfaces;
using ApiTemplate.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Get the connection string from the configuration (maybe environment variable for a docker container etc.)
        var useInMemoryDatabase = configuration.GetValue<bool>("USE_IN_MEMORY_DATABASE");
        var connectionString = configuration.GetValue<string>("CONNECTION_STRING");
        if (useInMemoryDatabase || string.IsNullOrWhiteSpace(connectionString))
            services.AddDbContext<ISampleDbContext, SampleDbContext>(options => options.UseInMemoryDatabase("ApiTemplate"));
        else
            services.AddDbContext<ISampleDbContext, SampleDbContext>(options => options.UseSqlServer(connectionString));
        
        return services;
    }
}