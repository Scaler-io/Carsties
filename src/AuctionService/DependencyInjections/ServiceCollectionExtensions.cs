using AuctionService.ConfigurationOptions.ElasticSearch;
using AuctionService.Data;
using AuctionService.Swagger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace AuctionService.DependencyInjections
{
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
                config.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(10);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });
                config.UsingRabbitMq((context, cfg) =>
                {
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

}
