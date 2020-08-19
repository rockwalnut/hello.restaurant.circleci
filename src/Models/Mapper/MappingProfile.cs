using System;
using AutoMapper;

using hello.restaurant.api.APIs.Model.Gateway;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PlaceAsync, Restaurant>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types))

                    .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Geometry.Location.Lat))
                    .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Geometry.Location.Lng))

                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.FormattedAddress))
                    .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))

                    .ReverseMap();

        }
    }
}