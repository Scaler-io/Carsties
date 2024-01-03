using GatewayService;
using GatewayService.ConfigurationOptions.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var identityGroupAccess = builder.Configuration
                .GetSection("IdentityGroupAccess")
                .Get<IdentityGroupAccessOptions>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = identityGroupAccess.Authority;
    options.Audience = identityGroupAccess.Audience;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = "username"
    };
});

var app = builder.Build();

try
{
    app.MapReverseProxy();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("DefaultPolicy");
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}