using AuctionService.Services;

namespace AuctionService.DependencyInjections;

public static class BusinessLogicExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAuctionService, Services.AuctionService>();
        return services;
    }
}
