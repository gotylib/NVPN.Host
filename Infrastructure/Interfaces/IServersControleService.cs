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
    bool AddServer(ServerDto server);
    
    /// <summary>
    /// Удаление сервера vpn.
    /// </summary>
    /// <param name="server">Модель сервера</param>
    /// <returns>Результат удаления</returns>
    bool RemoveServer(ServerDto server);
}