using MassTransit;
using NotificationService.Configurations.ServiceBus;

namespace NotificationService.DependencyInjections;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AdApplicationServices(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment env
    )
    {
        var logger = Logging.GetLogger(configuration, env);
        var rabbitMq = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

        services.AddSingleton(logger);

        // rabbitmq
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMq.Host, "/", host =>
                {
                    host.Username(rabbitMq.Username);
                    host.Password(rabbitMq.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
