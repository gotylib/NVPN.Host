using DAL;
using Infrastructure.Dto;

namespace Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с пользователем.
    /// </summary>
    public interface IUserControleService
    {
        /// <summary>
        /// Получить Jwt токен для пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <returns>Jwt токен.</returns>
        ResultModel<string, Exception> GenerateAccessToken(UserDto user);

        /// <summary>
        /// Зарегистрировать пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <returns>Jwt токен.</returns>
        ResultModel<string, Exception> RegisterUser(UserDto user);

        /// <summary>
        /// Войти в систему.
        /// </summary>
        /// <param name="user">/Модель пользователя.</param>
        /// <returns>Jwt токен.</returns>
        ResultModel<string, Exception> LoginUser(UserDto user);
    }
}
