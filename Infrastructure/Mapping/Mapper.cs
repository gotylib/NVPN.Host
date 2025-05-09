using AutoMapper;
using DAL.Entities;
using Infrastructure.Dto;

namespace Infrastructure.Mapping;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ServerModel, Server>()
            .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Salt, opt => opt.Ignore())
            .ReverseMap()
            .ForPath(desc => desc.Password, opt => opt.Ignore());

        CreateMap<User, RegisterModel>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.HashPassword));

    }
}