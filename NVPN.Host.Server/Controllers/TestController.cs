using AutoMapper;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NVPN.Host.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IServerCommandService serverCommandService) : ControllerBase
{
    private readonly IServerCommandService  _serverCommandService = serverCommandService;
    
}