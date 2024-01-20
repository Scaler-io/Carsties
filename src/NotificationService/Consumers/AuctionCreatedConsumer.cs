using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly ILogger _logger;
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionCreatedConsumer(ILogger logger,
        IHubContext<NotificationHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().Information("--> Auction created message received.");

        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);

        _logger.Here().Information("--> Auction created notification sent");
        _logger.Here().MethodExited();
    }
}
