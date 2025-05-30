using DAL.Entities;
using Infrastructure.AnswerObjects;
using Infrastructure.Dto;

namespace Infrastructure.Interfaces;

public interface IServersControleService
{
    /// <summary>
    /// Добавление нового сервера vpn.
    /// </summary>
    /// <param name="server">Модель сервера</param>
    /// <returns>Результат добавления</returns>
    Task<bool> AddServerAsync(ServerModel server);
    
    /// <summary>
    /// Удаление сервера vpn.
    /// </summary>
    /// <param name="id">Id сервера</param>
    /// <returns>Результат удаления</returns>
    Task<ResultModel<object, Exception>> RemoveServerAsync(int id);
    
    /// <summary>
    /// Обновить сервер vpn.
    /// </summary>
    /// <param name="server">Модель сервера</param>
    /// <returns>Результат обновления</returns>
    Task<bool> UpdateServerAsync(ServerModel server);
}