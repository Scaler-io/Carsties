using Serilog.Core;
using Serilog;
using Serilog.Events;
using Destructurama;
using SearchService.ConfigurationOptions.Logging;
using SearchService.ConfigurationOptions.App;
using SearchService.ConfigurationOptions.ElasticSearch;

namespace SearchService
{
    public class Logging
    {
        public static ILogger GetLogger(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var loggingOptions = configuration.GetSection("Logging").Get<LoggingOptions>();
            var appConfigurations = configuration.GetSection("AppConfigurations").Get<AppOptions>();
            var elasticUri = configuration.GetSection("Elasticsearch").Get<ElasticSearchOptions>();
            var logIndexPattern = $"Carsties.SearchService-{environment.EnvironmentName}";

            Enum.TryParse(loggingOptions.Console.LogLevel, false, out LogEventLevel minimumEventLevel);

            var loggerConfigurations = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumEventLevel))
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationIdentifier), appConfigurations.ApplicationIdentifier)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationEnvironment), appConfigurations.ApplicationEnvironment);

            if (loggingOptions.Console.Enabled)
            {
                loggerConfigurations.WriteTo.Console(minimumEventLevel, loggingOptions.LogOutputTemplate);
            }
            if (loggingOptions.Elastic.Enabled)
            {
                loggerConfigurations.WriteTo.Elasticsearch(elasticUri.Uri, logIndexPattern);
            }

            return loggerConfigurations
                   .Destructure
                   .UsingAttributes()
                   .CreateLogger();
        }
    }
}
