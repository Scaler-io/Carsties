using AutoMapper;
using BiddingService.Models;
using BiddingService.Models.DTOs;

namespace BiddingService.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bid, BidDto>();
    }
}
