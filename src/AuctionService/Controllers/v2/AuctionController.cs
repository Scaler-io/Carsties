using AuctionService.Controllers;
using AuctionService.Services;
using AuctionService.Swagger;
using Carsties.Shared.Extensions.Logger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuctionService;

[ApiVersion("2")]
public class AuctionController : BaseApiController
{
    private readonly IAuctionService _auctionService;
    public AuctionController(ILogger logger,
        IAuctionService auctionService) : base(logger)
    {
        _auctionService = auctionService;
    }

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllAuctions", Description = "Fetches all auctions")]
    public async Task<IActionResult> GetAllAuctions()
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.GetAllAuctions(RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAuction", Description = "Fetches auction by id")]
    public async Task<IActionResult> GetAuction([FromRoute] string id)
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.GetAuction(id, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
