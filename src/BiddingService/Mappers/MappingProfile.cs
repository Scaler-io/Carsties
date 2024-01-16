using AutoMapper;
using BiddingService.Models;
using BiddingService.Models.DTOs;
using Carsties.Shared.Contracts;

namespace BiddingService.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<Bid, BidPlaced>();
    }
}
