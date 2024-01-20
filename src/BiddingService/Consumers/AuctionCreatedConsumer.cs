using BiddingService.Models;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly ILogger _logger;

    public AuctionCreatedConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction created message processing started {auctionId}", context.Message.Id);

        var auction = new Auction
        {
            ID = context.Message.Id,
            Seller = context.Message.Seller,
            AuctionEnd = context.Message.AuctionEnd,
            ReservePrice = context.Message.ReservePrice
        };

        await auction.SaveAsync();

        _logger.Here().Information("Message processed successfully");
        _logger.Here().MethodExited();
    }
}
