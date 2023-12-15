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
        })).ReverseMap();

        CreateMap<Item, ItemDto>().ReverseMap();

        CreateMap<CreateAuctionDto, Auction>()
        .ForMember(d => d.Item, o => o.MapFrom(s => s)).ReverseMap();
        CreateMap<CreateAuctionDto, Item>().ReverseMap();

    }
}
