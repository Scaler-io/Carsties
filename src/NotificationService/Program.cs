using NotificationService.DependencyInjections;
using NotificationService.Hubs;
using NotificationService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdApplicationServices(
    builder.Configuration,
    builder.Environment
);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapHub<NotificationHub>("/notification");

app.Run();
