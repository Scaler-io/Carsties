using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly ILogger _logger;

    public BidPlacedConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().Information("Bidplaced message processing started");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (auction is null)
        {
            _logger.Here().Warning("No auction is present in search database with id {auctionId}", context.Message.AuctionId);
            return;
        }

        if (context.Message.BidStatus.Contains("Accepted")
        && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }

        _logger.Here().Information("Message processed successfully");
        _logger.Here().MethodExited();
    }
}
