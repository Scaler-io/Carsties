using AuctionService;
using AuctionService.Data;
using AuctionService.DependencyInjections;
using AuctionService.Extensions;
using AuctionService.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;

builder.Services.AddApplicationServices(configurations)
                .AddDatabaseServices(configurations)
                .AddBusinessLogicServices(configurations);

var logger = Logging.GetLogger(configurations, builder.Environment);
var host = builder.Host.UseSerilog(logger);

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Carsties auction api - {description.GroupName.ToUpperInvariant()}");
    }
});

app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestLoggerMiddleware>();
app.UseMiddleware<CorrelationHeaderEnricher>();
app.UseMiddleware<GLobalExceptionMiddleware>();

try
{
    app.MigrateDb<AuctionDbContext>((context, services) =>
    {
        var logger = services.GetRequiredService<ILogger>();
        var environment = services.GetRequiredService<IWebHostEnvironment>();
        AuctionDbContextSeed.SeedAsync(context, logger, environment).Wait();
    });
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
