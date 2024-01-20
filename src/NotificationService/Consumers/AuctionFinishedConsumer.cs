using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly ILogger _logger;
    private readonly IHubContext<NotificationHub> _hubcontext;

    public AuctionFinishedConsumer(ILogger logger, IHubContext<NotificationHub> hubcontext)
    {
        _logger = logger;
        _hubcontext = hubcontext;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().Information("--> Auction finished message received");

        await _hubcontext.Clients.All.SendAsync("AuctionFinished", context.Message);

        _logger.Here().Information("--> Auction finished notification sent");
        _logger.Here().MethodExited();
    }
}
