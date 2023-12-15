using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.DependencyInjections;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("CarstiesConnection"));
        });
        return services;
    }
}
