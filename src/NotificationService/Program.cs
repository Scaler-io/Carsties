using NotificationService.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdApplicationServices(
    builder.Configuration,
    builder.Environment
);

var app = builder.Build();

app.Run();
