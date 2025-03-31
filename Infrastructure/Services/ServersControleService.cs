using AutoMapper;
using DAL.Context;
using Infrastructure.Dto;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ServersControleService : IServersControleService
{
    private readonly IMapper _mapper;
    
    private readonly VpnDbContext _context;
    
    public ServersControleService(IMapper mapper, VpnDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public bool AddServer(ServerDto server)
    {
        throw new NotImplementedException();
    }

    public bool RemoveServer(ServerDto server)
    {
        throw new NotImplementedException();
    }
}