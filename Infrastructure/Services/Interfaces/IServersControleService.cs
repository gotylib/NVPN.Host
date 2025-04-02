using DAL.Entities;
using Infrastructure.Dto;

namespace Infrastructure.Interfaces;

public interface IServersControleService
{
    /// <summary>
    /// Добавление нового сервера vpn.
    /// </summary>
    /// <param name="server">Модель сервера</param>
    /// <returns>Результат добавления</returns>
    Task<bool> AddServerAsync(ServerDto server);
    
    /// <summary>
    /// Удаление сервера vpn.
    /// </summary>
    /// <param name="id">Id сервера</param>
    /// <returns>Результат удаления</returns>
    Task<bool> RemoveServerAsync(int id);
    
    /// <summary>
    /// Обновить сервер vpn.
    /// </summary>
    /// <param name="server">Модель сервера</param>
    /// <returns>Результат обновления</returns>
    Task<bool> UpdateServerAsync(ServerDto server);
}