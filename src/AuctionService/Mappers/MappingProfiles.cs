using AuctionService.Entities;
using AuctionService.Models.DTOs;
using AutoMapper;
using Carsties.Shared.Contracts;

namespace AuctionService.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>()
        .ForMember(d => d.AuctionEnd, o => o.MapFrom(s => s.AuctionEnd.ToString()))
        .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
        {
            CreatedAt = s.CreatedAt.ToString(),
            UpdatedAt = s.UpdatedAt.ToString()
        })).ReverseMap();

        CreateMap<Item, ItemDto>().ReverseMap();

        CreateMap<CreateAuctionDto, Auction>()
        .ForMember(d => d.Item, o => o.MapFrom(s => s)).ReverseMap();
        CreateMap<CreateAuctionDto, Item>().ReverseMap();

        CreateMap<UpdateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s)).ReverseMap();
        CreateMap<UpdateAuctionDto, Item>().ReverseMap();

        CreateMap<AuctionDto, AuctionCreated>()
        .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
        .ForMember(d => d.Make, o => o.MapFrom(s => s.Item.Make))
        .ForMember(d => d.Model, o => o.MapFrom(s => s.Item.Model))
        .ForMember(d => d.Year, o => o.MapFrom(s => s.Item.Year))
        .ForMember(d => d.Color, o => o.MapFrom(s => s.Item.Color))
        .ForMember(d => d.Mileage, o => o.MapFrom(s => s.Item.Mileage))
        .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Item.ImageUrl))
        .ForMember(d => d.AuctionEnd, o => o.MapFrom(s => DateTime.Parse(s.AuctionEnd).ToUniversalTime()))
        .ForMember(d => d.CreatedAt, o => o.MapFrom(s => DateTime.Parse(s.MetaData.CreatedAt).ToUniversalTime()))
        .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => DateTime.Parse(s.MetaData.UpdatedAt).ToUniversalTime()));
    }
}
