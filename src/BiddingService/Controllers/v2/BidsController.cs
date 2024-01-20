using BiddingService.Services;
using BiddingService.Swagger;
using Carsties.Shared.Extensions.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BiddingService.Controllers.v2;

[ApiVersion("2")]
public class BidsController : BaseApiController
{
    private IBidService _bidService;
    public BidsController(ILogger logger, IBidService bidService)
        : base(logger)
    {
        _bidService = bidService;
    }

    [HttpGet("{auctionId}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id", Type = "string")]
    [SwaggerOperation(OperationId = "GetBidsForAuction", Description = "Fetches all bids for auction")]
    public async Task<IActionResult> GetBidsForAuction([FromRoute] string auctionId)
    {
        Logger.Here().MethodEnterd();
        var result = await _bidService.GetAuctionBids(auctionId, RequestInformation);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost]
    [Authorize]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id", Type = "string")]
    [SwaggerOperation(OperationId = "PlaceBid", Description = "Places a new bid against auction")]
    public async Task<IActionResult> PlaceBid(string auctionId, int amount)
    {
        Logger.Here().MethodEnterd();
        var result = await _bidService.PlaceNewBid(auctionId, amount, RequestInformation);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
