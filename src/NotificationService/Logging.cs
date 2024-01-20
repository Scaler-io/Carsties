using Destructurama;
using NotificationService.Configurations.App;
using NotificationService.Configurations.ElasticSearch;
using NotificationService.Configurations.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace NotificationService;

public class Logging
{
    public static ILogger GetLogger(IConfiguration configuration, IWebHostEnvironment env)
    {
        var loggingOptions = configuration.GetSection("Logging").Get<LoggingOptions>();
        var appOptions = configuration.GetSection("AppConfigurations").Get<AppOptions>();
        var elasticOptions = configuration.GetSection("Elasticsearch").Get<ElasticSearchOptions>();
        var logIndexPattern = $"Carsties.NotificationService-{env.EnvironmentName}";

        Enum.TryParse(loggingOptions.Console.LogLevel, false, out LogEventLevel minimumEventLevel);

        var loggerConfigurations = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumEventLevel))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
            .Enrich.WithProperty(nameof(appOptions.ApplicationIdentifier), appOptions.ApplicationIdentifier)
            .Enrich.WithProperty(nameof(appOptions.ApplicationEnvironment), appOptions.ApplicationEnvironment);

        if (loggingOptions.Console.Enabled)
        {
            loggerConfigurations.WriteTo.Console(minimumEventLevel, loggingOptions.LogOutputTemplate);
        }
        if (loggingOptions.Elastic.Enabled)
        {
            loggerConfigurations.WriteTo.Elasticsearch(elasticOptions.Uri, logIndexPattern);
        }

        return loggerConfigurations
                .Destructure
                .UsingAttributes()
                .CreateLogger();
    }
}
