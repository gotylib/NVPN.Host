using AutoMapper;
using DAL.Entities;
using Infrastructure.Dto;

namespace Infrastructure.Mapping;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ServerDto, Server>().ReverseMap();
    }
}