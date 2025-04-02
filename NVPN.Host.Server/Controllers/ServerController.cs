using AutoMapper;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace NVPN.Host.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerController : ControllerBase
{
    IServersControleService _serversControleService;

    public ServerController(IServersControleService serversControleService)
    {
        _serversControleService = serversControleService;
    }
    
    [HttpPost("AddServer")]
    public async Task<ActionResult> AddServer(ServerDto server)
    {
        return await _serversControleService.AddServerAsync(server) ? Ok() : BadRequest();
    }

    [HttpPut("UpdateServer")]
    public async Task<ActionResult> UpdateServer(ServerDto server)
    {
        return await _serversControleService.UpdateServerAsync(server) ? Ok() : BadRequest();
    }
        
    [HttpDelete("RemoveServer")]
    public async Task<ActionResult> DeleteServer(int id)
    {
        return await _serversControleService.RemoveServerAsync(id) ? Ok() : BadRequest();
    }
}