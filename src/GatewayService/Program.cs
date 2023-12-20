using GatewayService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReversProxy"));

var app = builder.Build();

try
{
    app.MapReverseProxy();
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}