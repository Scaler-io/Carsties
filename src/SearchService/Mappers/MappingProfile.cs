using AutoMapper;
using Carsties.Shared.Contracts;
using SearchService.Entities;

namespace SearchService.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuctionCreated, Item>();
        CreateMap<AuctionUpdated, Item>();
    }
}
