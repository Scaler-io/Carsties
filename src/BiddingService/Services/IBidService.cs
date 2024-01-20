using BiddingService.Models.DTOs;
using Carsties.Shared.Models.Core;

namespace BiddingService.Services;

public interface IBidService
{
    Task<Result<IReadOnlyList<BidDto>>> GetAuctionBids(string auctionId, RequestInformation requestInformation);
    Task<Result<BidDto>> PlaceNewBid(string auctionId, int amount, RequestInformation requestInformation);
}
