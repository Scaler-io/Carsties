using BiddingService.ConfigurationOptions.ElasticSearch;
using BiddingService.ConfigurationOptions.Identity;
using BiddingService.ConfigurationOptions.ServiceBus;
using BiddingService.Consumers;
using BiddingService.Services.Background;
using BiddingService.Services.Grpc;
using BiddingService.Swagger;
using Carsties.Shared.Contracts;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace BiddingService.DependencyInjections;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(true);
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var scheme = configuration["SwaggerConfig:Scheme"];
            var url = configuration["SwaggerConfig:Host"];
            options.EnableAnnotations();
            options.ExampleFilters();
            options.DocumentFilter<SwaggerApiVersionFilter>();
            options.OperationFilter<SwaggerHeaderFilter>();
            options.AddServer(new OpenApiServer { Url = $"{scheme}://{url}" });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            });
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.Configure<ElasticSearchOptions>(configuration.GetSection("ElasticSearch"));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));
            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitmq = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
                cfg.Host(rabbitmq.Host, "/", host =>
                {
                    host.Username(rabbitmq.Username);
                    host.Password(rabbitmq.Password);
                });
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

        var identityGroupAccess = configuration
                .GetSection("IdentityGroupAccess")
                .Get<IdentityGroupAccessOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = identityGroupAccess.Authority;
            options.Audience = identityGroupAccess.Audience;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "username"
            };
        });

        services.AddHostedService<AuctionFinishedBackgroundService>();
        services.AddScoped<GrpcAuctionClient>();

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


