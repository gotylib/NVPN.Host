using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ServersControleService : IServersControleService
{
    private readonly IMapper _mapper;
    
    private readonly VpnDbContext _context;
    
    private readonly ILogger<ServersControleService> _logger;
    
    public ServersControleService(IMapper mapper, VpnDbContext context, ILogger<ServersControleService> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }
    
    public async Task<bool> AddServerAsync(ServerDto server)
    {
        try
        {
            var serverEntity = _mapper.Map<Server>(server);
            _context.Servers.Add(serverEntity);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Это тестовое исключение для проверки логирования.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> RemoveServerAsync(int id)
    {
        try
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return true;
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;   
        }
    }

    public async Task<bool> UpdateServerAsync(ServerDto server)
    {
        try
        {
            var serverEntity = await _context.Servers.FindAsync(server.Id);

            if (serverEntity == null)
            {
                await AddServerAsync(server);
            }

            _mapper.Map(serverEntity, server);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }
}