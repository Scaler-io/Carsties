using SearchService.Services;

namespace SearchService.DependencyInjections;

public static class BusinessLogicExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, Services.SearchService>();
        return services;
    }
}
