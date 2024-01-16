using BiddingService.Services;

namespace BiddingService.DependencyInjections;

public static class BusinesslogicExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IBidService, BidService>();
        return services;
    }
}
