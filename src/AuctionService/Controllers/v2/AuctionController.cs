using AuctionService.Controllers;
using AuctionService.Models.DTOs;
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
    public async Task<IActionResult> GetAllAuctions([FromQuery] string date)
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.GetAllAuctions(date, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}", Name = "GetAuction")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAuction", Description = "Fetches auction by id")]
    public async Task<IActionResult> GetAuction([FromRoute] string id)
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.GetAuction(id, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateAuction", Description = "Creates new auction")]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto createAuction)
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.CreateAuction(createAuction, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return CreatedResult(result, nameof(GetAuction), new { id = result.Value.Id.ToString() });
    }

    [HttpPut("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "UpdateAuction", Description = "Updates new auction")]
    public async Task<IActionResult> UpdateAuction([FromRoute] string id, [FromBody] UpdateAuctionDto updateAuction)
    {
        Logger.Here().MethodEnterd();
        var result = await _auctionService.UpdateAuction(id, updateAuction, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
