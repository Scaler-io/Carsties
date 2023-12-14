using AuctionService;
using AuctionService.DependencyInjections;
using AuctionService.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;

builder.Services.AddApplicationServices(configurations)
                .AddDatabaseServices(configurations);

var logger = Logging.GetLogger(configurations, builder.Environment);
var host = builder.Host.UseSerilog(logger);

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Carsties auction api - {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestLoggerMiddleware>();
app.UseMiddleware<CorrelationHeaderEnricher>();
app.UseMiddleware<GLobalExceptionMiddleware>();

app.Run();
