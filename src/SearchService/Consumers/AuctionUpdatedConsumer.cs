using AutoMapper;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(ILogger logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .ForContext("MessageId", context.MessageId)
            .Debug("Auction updated message processing started {auctionId}", context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);
        var result = await DB.Update<Item>()
                        .Match(a => a.ID == context.Message.Id)
                        .ModifyOnly(x => new
                        {
                            x.Color,
                            x.Make,
                            x.Model,
                            x.Year,
                            x.Mileage
                        }, item).ExecuteAsync();

        if (!result.IsAcknowledged)
        {
            _logger.Here().Error("Problem updating mongo database");
            throw new MessageException(typeof(AuctionUpdated), "Problem updating mongo database");
        }

        _logger.Here().Debug("Message proccessing completed");
        _logger.Here().MethodExited();
    }
}
