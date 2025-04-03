using Infrastructure.SafetyService;
using Microsoft.AspNetCore.Mvc;

namespace NVPN.Host.Server.Controllers;

/// <summary>
/// Контроллер для управления генерацией приватных ключей
/// </summary>
[ApiController]
[Route("[controller]")]
public class SafetyController: ControllerBase
{
    private readonly SafetyService _safetyService;
    
    public SafetyController(SafetyService safetyService)
    {
        _safetyService = safetyService;
    }

    [HttpGet("GenerateAdminKey")]
    public ActionResult<Guid> GenerateAdminKey()
    {
        if (_safetyService.IsGenerate)
        {
            return BadRequest("Admin Key is already generated");
        }
        _safetyService.AdminKey = Guid.NewGuid();
        _safetyService.IsGenerate = true;
        return Ok(_safetyService.AdminKey);
    }
}