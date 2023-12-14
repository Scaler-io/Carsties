using AuctionService.Entities;
using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Helpers;

namespace AuctionService.Data;

public class AuctionDbContextSeed
{
    public static async Task SeedAsync(AuctionDbContext dbContext, ILogger logger, IWebHostEnvironment env)
    {
        if (dbContext.Auctions.Any())
        {
            logger.Here().Information("Auction data is already present. No need to seed");
            return;
        }

        dbContext.Auctions.AddRange(GetSampleAuctionData(env, logger));
        await dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Auction> GetSampleAuctionData(IWebHostEnvironment env, ILogger logger)
    {
        var path = Path.Combine(env.ContentRootPath, "App_Data/SeedData");
        logger.Here().Information("data collected from {@path}", path);
        var auctions = FileReader<Auction>.SeederFileReader("AuctionSample", path);
        return auctions;
    }
}
