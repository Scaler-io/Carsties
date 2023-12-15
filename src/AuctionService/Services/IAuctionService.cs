using AuctionService.Models.DTOs;
using Carsties.Shared.Models.Core;

namespace AuctionService.Services;

public interface IAuctionService
{
    Task<Result<IReadOnlyList<AuctionDto>>> GetAllAuctions(string correlationId);
    Task<Result<AuctionDto>> GetAuction(string id, string correlationId);
}
