using BiddingService;
using BiddingService.DependencyInjections;
using BiddingService.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);

builder.Services.AddSingleton(logger);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
            $"Carsties bidding api - {description.GroupName.ToUpperInvariant()}");
    }
});

app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

app.UseMiddleware<RequestLoggerMiddleware>();

app.UseMiddleware<CorrelationHeaderEnricher>();

app.UseMiddleware<GLobalExceptionMiddleware>();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
