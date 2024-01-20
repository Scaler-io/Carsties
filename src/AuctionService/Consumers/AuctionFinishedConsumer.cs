using AuctionService.Data;
using AuctionService.Entities;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly ILogger _logger;
    private readonly AuctionDbContext _context;

    public AuctionFinishedConsumer(ILogger logger,
        AuctionDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction finished message processing started {auctionId}", context.Message.AuctionId);

        var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));
        if (auction is null)
        {
            _logger.Here().Warning("No auction is present in database with id {auctionId}", context.Message.AuctionId);
            return;
        }

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        auction.Status = auction.SoldAmount > auction.ReservePrice
            ? Status.Finished : Status.ReserveNotMet;

        await _context.SaveChangesAsync();

        _logger.Here().Information("Message processed successfully");
        _logger.Here().MethodExited();
    }
}
