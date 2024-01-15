using BiddingService;
using BiddingService.DependencyInjections;
using BiddingService.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MongoDB.Driver;
using MongoDB.Entities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Services.AddSingleton(x => logger);

builder.Services.AddApplicationServices(builder.Configuration);

await DB.InitAsync("BidDb", MongoClientSettings
        .FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

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

app.UseAuthentication();
app.UseAuthorization();

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
