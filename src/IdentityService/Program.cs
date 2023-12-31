﻿using Carsties.Shared.Extensions.Logger;
using IdentityService;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);

logger.Information("Starting up");

try
{
    var host = builder.Host.UseSerilog(logger);

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.
    SeedData.EnsureSeedData(app);

    app.Run();
}
catch (Exception ex) when (
                // https://github.com/dotnet/runtime/issues/60600
                ex.GetType().Name is not "StopTheHostException"
                // HostAbortedException was added in .NET 7, but since we target .NET 6 we
                // need to do it this way until we target .NET 8
                && ex.GetType().Name is not "HostAbortedException"
                        )
{
    logger.Here().Fatal(ex, "Unhandled exception");
}
finally
{
    logger.Here().Information("Shut down complete");
    Log.CloseAndFlush();
}