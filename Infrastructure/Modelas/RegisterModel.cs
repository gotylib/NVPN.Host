namespace Infrastructure.Dto;

/// <summary>
/// Модель для регистрации.
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// Имя пользователя полностью. (ФИО).
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; }
}