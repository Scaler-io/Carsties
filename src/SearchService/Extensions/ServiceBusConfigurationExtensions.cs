using MassTransit;

namespace SearchService.Extensions;

public static class ServiceBusConfigurationExtensions
{
    public static void ConfigureRecieveEndpoint<T>(this IRabbitMqBusFactoryConfigurator cfg,
        string endpoint, IBusRegistrationContext context
    ) where T : class, IConsumer
    {
        cfg.ReceiveEndpoint(endpoint, e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<T>(context);
        });
    }
}
