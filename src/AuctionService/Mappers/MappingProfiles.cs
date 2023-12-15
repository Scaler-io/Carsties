using AuctionService.Entities;
using AuctionService.Models.DTOs;
using AutoMapper;

namespace AuctionService.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>()
        .ForMember(d => d.AuctionEnd, o => o.MapFrom(s => s.AuctionEnd.ToString("dd/MM/yyyy hh:mm:ss tt")))
        .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
        {
            CreatedAt = s.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss tt"),
            UpdatedAt = s.UpdatedAt.ToString("dd/MM/yyyy hh:mm:ss tt")
        }));

        CreateMap<Item, ItemDto>();

        CreateMap<CreateAuctionDto, Auction>()
        .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();

    }
}
