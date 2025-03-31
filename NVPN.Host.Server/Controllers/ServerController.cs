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
    
    [HttpGet("AddServer")]
    public ActionResult AddServer()
    {
        return Ok("Hello");
    }
}