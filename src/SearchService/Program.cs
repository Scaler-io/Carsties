using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SearchService;
using SearchService.DependencyInjections;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configurations = builder.Configuration;

builder.Services.AddApplicationServices(configurations);

var logger = Logging.GetLogger(configurations, builder.Environment);
var host = builder.Host.UseSerilog(logger);

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Carsties search api - {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
