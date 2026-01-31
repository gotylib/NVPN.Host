using Infrastructure.Dto;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NVPN.Host.Server.Controllers;

/// <summary>
/// OData контроллер для работы с пользователями.
/// Использует сервис UserControlService для получения данных.
/// </summary>
public class UsersController(IUserControlService userControlService) : ODataController
{
    /// <summary>
    /// Получить список пользователей с поддержкой OData запросов.
    /// Примеры запросов:
    /// - /odata/Users - получить всех пользователей
    /// - /odata/Users?$filter=Username eq 'John' - фильтрация
    /// - /odata/Users?$orderby=Username desc - сортировка
    /// - /odata/Users?$select=Id,Username,Email - выборка полей
    /// - /odata/Users?$top=10&$skip=20 - пагинация
    /// </summary>
    [EnableQuery(MaxExpansionDepth = 3, MaxAnyAllExpressionDepth = 3)]
    public IActionResult Get()
    {
        // Получаем IQueryable из сервиса
        // OData middleware автоматически применит параметры запроса ($filter, $orderby и т.д.)
        // к этому IQueryable перед выполнением запроса к БД
        var users = userControlService.GetUsersQueryable();
        return Ok(users);
    }

    /// <summary>
    /// Получить пользователя по ID.
    /// Пример: /odata/Users(1)
    /// </summary>
    [EnableQuery]
    public IActionResult Get(int key)
    {
        var user = userControlService.GetUsersQueryable().FirstOrDefault(u => u.Id == key);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] UserModel user)
    {
        var result = await userControlService.AddUser(user);
        
        if (result.IsSuccessful) return Ok(result.Value);
        if (result.Error != null) throw result.Error;

        return Ok(result.Value);
    }
}