using AuctionService;
using BiddingService.Models;
using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using Grpc.Net.Client;

namespace BiddingService.Services.Grpc;

public class GrpcAuctionClient
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public GrpcAuctionClient(ILogger logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Result<Auction> GetAuction(string id, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId)
            .Information("Calling grpc auction service");

        var channel = GrpcChannel.ForAddress(_configuration["GrpcAuction"]);
        var client = new GrpcAuction.GrpcAuctionClient(channel);

        var request = new GetAuctionRequest { Id = id };

        try
        {
            var reply = client.GetAuction(request);
            var auction = new Auction
            {
                ID = reply.Auction.Id,
                AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                Seller = reply.Auction.Seller,
                ReservePrice = reply.Auction.ReservePrice,
            };

            _logger.Here().Information("Auction fetched {@auction}", auction);
            _logger.Here().MethodExited();
            return Result<Auction>.Success(auction);
        }
        catch (Exception ex)
        {
            _logger.Here()
            .WithCorrelationId(correlationId)
            .Error("Failed to fetch auction details");
            
            return Result<Auction>.Failure(ErrorCodes.OperationFailed, ex.Message);
        }
    }
}
