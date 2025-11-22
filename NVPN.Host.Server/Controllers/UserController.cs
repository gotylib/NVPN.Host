using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NVPN.Host.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserControlService userControleService) : ControllerBase
{
    private readonly IUserControlService _userControleService = userControleService;
}