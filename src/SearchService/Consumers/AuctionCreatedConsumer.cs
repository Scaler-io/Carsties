using AutoMapper;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(ILogger logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction created message processing started {auctionId}", context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        await item.SaveAsync();

        _logger.Here().Debug("Message proccessing completed");
        _logger.Here().MethodExited();
    }
}
