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
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}