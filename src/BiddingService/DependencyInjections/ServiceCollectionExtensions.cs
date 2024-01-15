using BiddingService.ConfigurationOptions.ElasticSearch;
using BiddingService.Swagger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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


