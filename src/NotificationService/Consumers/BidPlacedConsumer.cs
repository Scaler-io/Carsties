using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly ILogger _logger;
    private readonly IHubContext<NotificationHub> _hubContext;

    public BidPlacedConsumer(ILogger logger, IHubContext<NotificationHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().Information("--> Bid placed messgae received");

        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);

        _logger.Here().Information("--> Bid placed notification sent");
        _logger.Here().MethodExited();
    }
}
