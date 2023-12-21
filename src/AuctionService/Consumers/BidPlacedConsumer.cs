using AuctionService.Data;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly ILogger _logger;
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(ILogger logger,
        AuctionDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().Information("Bidplaced message processing started");

        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);
        if (auction is null)
        {
            _logger.Here().Warning("No auction is present in database with id {auctionId}", context.Message.AuctionId);
            return;
        }

        if (auction.CurrentHighBid == null
        || context.Message.BidStatus.Contains("Accepted")
        && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await _dbContext.SaveChangesAsync();
        }

        _logger.Here().Information("Message processed successfully");
        _logger.Here().MethodExited();
    }
}
