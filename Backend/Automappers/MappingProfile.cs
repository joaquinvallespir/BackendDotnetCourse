using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Automappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BeerInsertDto, Beer>();
            CreateMap<Beer, BeerDto>().ForMember(dto => dto.Id, m => m.MapFrom(b => b.BeerId));
            CreateMap<BeerUpdateDto, Beer>();
            CreateMap<Sale, SaleDto>().ForMember(dto => dto.Id, m => m.MapFrom(s => s.SaleId));
            CreateMap<SaleInsertDto, Sale>();
            CreateMap<SaleUpdateDto, Sale>();
            CreateMap<Brand, BrandDto>().ForMember(dto => dto.Id, m => m.MapFrom(s => s.BrandId));
            CreateMap<BrandInsertDto, Brand>();
            CreateMap<BrandUpdateDto, Brand>();
        }

    }
}
