using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    private readonly ILogger _logger;

    public AuctionDeletedConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction deleted message processing started {auctionId}", context.Message.Id);

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        if (!result.IsAcknowledged)
        {
            _logger.Here().Error("Problem deleting item from mongo database");
            throw new MessageException(typeof(AuctionUpdated), "Problem deleting item from mongo database");
        }

        _logger.Here().Debug("Message proccessing completed");
        _logger.Here().MethodExited();
    }
}
