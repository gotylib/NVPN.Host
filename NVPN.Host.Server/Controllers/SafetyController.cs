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
    private readonly Safety _safety;
    
    public SafetyController(Safety safety)
    {
        _safety = safety;
    }

    [HttpGet("GenerateAdminKey")]
    public ActionResult<Guid> GenerateAdminKey()
    {
        if (_safety.IsGenerate)
        {
            return BadRequest("Admin Key is already generated");
        }
        _safety.AdminKey = Guid.NewGuid();
        _safety.IsGenerate = true;
        return Ok(_safety.AdminKey);
    }
}