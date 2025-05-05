using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Infrastructure.AnswerObjects;
using Microsoft.AspNetCore.Mvc;
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
    
    public async Task<bool> AddServerAsync(ServerModel server)
    {
        try
        {
            var serverEntity = _mapper.Map<Server>(server);
            _context.Servers.Add(serverEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<ResultModel<object, Exception>> RemoveServerAsync(int id)
    {
        try
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
               return ResultModel<object, Exception>.CreateFailedResult();
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
            return ResultModel<object, Exception>.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            return ResultModel<object, Exception>.CreateFailedResult(ex);
        }
    }

    public async Task<bool> UpdateServerAsync(ServerModel server)
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