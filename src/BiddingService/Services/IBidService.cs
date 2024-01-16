using BiddingService.Models;
using Carsties.Shared.Models.Core;

namespace BiddingService.Services;

public interface IBidService
{
    Task<Result<IReadOnlyList<Bid>>> GetAuctionBids(string auctionId, RequestInformation requestInformation);
    Task<Result<Bid>> PlaceNewBid(string auctionId, int amount, RequestInformation requestInformation);
}
