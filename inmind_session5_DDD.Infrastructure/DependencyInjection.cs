
using inmind_session5_DDD.Persistence;
using Microsoft.Extensions.Configuration;

namespace inmind_session5_DDD.Infrastructure;


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddSingleton<BlobStorageService>();
        
      



        return services;
    }
}
