using System.Reflection;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using SearchService.ConfigurationOptions.ElasticSearch;
using SearchService.Consumers;
using SearchService.Extensions;
using SearchService.Swagger;
using Swashbuckle.AspNetCore.Filters;

namespace SearchService.DependencyInjections;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(option =>
            {
                option.UseCamelCasing(true);
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.OperationFilter<SwaggerHeaderFilter>();
            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.Configure<ElasticSearchOptions>(configuration.GetSection("ElasticSearch"));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRecieveEndpoint<AuctionCreatedConsumer>("search-auction-created", context);
                cfg.ConfigureRecieveEndpoint<AuctionFinishedConsumer>("search-auction-finished", context);
                cfg.ConfigureRecieveEndpoint<AuctionUpdatedConsumer>("search-auction-updated", context);
                cfg.ConfigureRecieveEndpoint<BidPlacedConsumer>("search-bid-placed", context);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddHttpContextAccessor();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = HandleFrameworkValidationFailure();
        });
        return services;
    }

    private static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
    {
        return context =>
        {
            var errors = context.ModelState
                                .Where(err => err.Value.Errors.Count > 0)
                                .ToList();

            var validationError = new ApiValidationResponse
            {
                Errors = new List<FieldLevelError>()
            };
            foreach (var error in errors)
            {
                var fieldLevelError = new FieldLevelError()
                {
                    Code = ErrorCodes.BadRequest.ToString(),
                    Field = error.Key,
                    Message = error.Value.Errors?.First().ErrorMessage,
                };

                validationError.Errors.Add(fieldLevelError);
            }
            return new BadRequestObjectResult(validationError);
        };
    }
}
