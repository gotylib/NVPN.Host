using DAL;
using DAL.Entities;
using Infrastructure.Dto;

namespace Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с пользователем.
    /// </summary>
    public interface IUserControlService
    {
        /// <summary>
        /// Добавть нового пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        /// <returns></returns>
        Task<ResultModel<string, Exception>> AddUser(UserModel user);
        
        /// <summary>
        /// Обновить данные пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        /// <returns></returns>
        Task<ResultModel<string, Exception>> UpdateUser(UserModel user);
        
        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        Task<ResultModel<bool, Exception>> DeleteUser(int id);
        
        /// <summary>
        /// Получить список пользователей
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<UserModel>?, Exception>> GetUsers();
        
        /// <summary>
        /// Получить IQueryable пользователей для OData запросов.
        /// OData middleware автоматически применит фильтры, сортировку и другие параметры к этому запросу.
        /// </summary>
        /// <returns>IQueryable для работы с OData</returns>
        IQueryable<User> GetUsersQueryable();
        
        /// <summary>
        /// Получить пользователя по id.
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        Task<ResultModel<UserModel?, Exception>> GetUserById(int id);
    }
}
