using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Extensions;

public static class WebApplictionExtensions
{
    public static WebApplication MigrateDb<TContext>(this WebApplication app,
    Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
    {
        int retryForAvailability = retry.Value;
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.Here().Information("Migrating database with context {@DbContextName}", typeof(TContext).Name);

                InvokeSeeder(seeder, context, services);

                logger.Here().Information("Migrated database with context {@DbContextName}", typeof(TContext).Name);

            }
            catch (Exception ex)
            {
                logger.Here().Error("{@ErrorCode} Migration failed. {@Message} - {@StackTrace}", ErrorCodes.OperationFailed, ex.Message, ex.StackTrace);
                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDb<TContext>(app, seeder, retryForAvailability);
                }
            }
        }

        return app;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
