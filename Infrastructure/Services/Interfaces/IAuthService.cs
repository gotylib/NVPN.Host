using Infrastructure.AnswerObjects;
using Infrastructure.Dto;

namespace Infrastructure.Interfaces;

/// <summary>
/// Сервис для аутентификации.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Получить Jwt токен для пользователя.
    /// </summary>
    /// <param name="user">Модель пользователя.</param>
    /// <returns>Jwt токен.</returns>
    ResultModel<string, Exception> GenerateAccessToken(UserModel user);

    /// <summary>
    /// Зарегистрировать пользователя.
    /// </summary>
    /// <param name="user">Модель пользователя.</param>
    /// <returns>Jwt токен.</returns>
    ResultModel<string, Exception> RegisterUser(RegisterModel user);

    /// <summary>
    /// Войти в систему.
    /// </summary>
    /// <param name="user">/Модель пользователя.</param>
    /// <returns>Jwt токен.</returns>
    ResultModel<string, Exception> LoginUser(LoginModel user);
}