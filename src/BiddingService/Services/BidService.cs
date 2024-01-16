using BiddingService.Models;
using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MongoDB.Entities;

namespace BiddingService.Services;

public class BidService : IBidService
{
    private readonly ILogger _logger;

    public BidService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<Bid>>> GetAuctionBids(string auctionId, RequestInformation requestInformation)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Information("Request - get all bids for auction {auctionId}", auctionId);

        var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(a => a.BidTime))
            .ExecuteAsync();

        if (!bids.Any())
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
            .Error("No bids were found for the auction {auctionId}", auctionId);
            return Result<IReadOnlyList<Bid>>.Failure(ErrorCodes.NotFound);
        }

        _logger.Here().MethodExited();
        return Result<IReadOnlyList<Bid>>.Success(bids);
    }
    public async Task<Result<Bid>> PlaceNewBid(string auctionId, int amount, RequestInformation requestInformation)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
                .Information("Request - place bid of {amount} for auction {auctionId}", auctionId, amount);

        var auction = await DB.Find<Auction>().OneAsync(auctionId);
        if (auction is null)
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
                .Error("No auction was found with id {auctionId}", auctionId);
            return Result<Bid>.Failure(ErrorCodes.NotFound);
        }

        if (auction.Seller == requestInformation.CurrentUser.Name)
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
            .Error("user cannot bid on own auction");
            return Result<Bid>.Failure(ErrorCodes.BadRequest);
        }

        var bid = await PrepareNewBid(auctionId, amount, requestInformation, auction);
        await DB.SaveAsync(bid);

        _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
            .Information("New bid has been placed");
        _logger.Here().MethodExited();

        return Result<Bid>.Success(bid);
    }

    private static async Task<Bid> PrepareNewBid(string auctionId, int amount, RequestInformation requestInformation, Auction auction)
    {
        var bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = requestInformation.CurrentUser.Name
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }

        var highestAvailableBid = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(x => x.Amount))
            .ExecuteFirstAsync();

        if (highestAvailableBid != null && amount > highestAvailableBid.Amount || highestAvailableBid == null)
        {
            bid.BidStatus = amount > auction.ReservePrice
                            ? BidStatus.Accepted
                            : BidStatus.AcceptedBelowReserve;
        }
        if (highestAvailableBid != null && amount < highestAvailableBid.Amount)
        {
            bid.BidStatus = BidStatus.TooLow;
        }

        return bid;
    }


}
