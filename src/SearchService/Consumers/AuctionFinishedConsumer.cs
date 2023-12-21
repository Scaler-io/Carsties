using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly ILogger _logger;

    public AuctionFinishedConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction finished message processing started {auctionId}", context.Message.AuctionId);

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (auction is null)
        {
            _logger.Here().Warning("No auction is present in search database with id {auctionId}", context.Message.AuctionId);
            return;
        }

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = (int)context.Message.Amount;
        }

        auction.Status = "Finished";
        await auction.SaveAsync();

        _logger.Here().Information("Message processed successfully");
        _logger.Here().MethodExited();
    }
}
