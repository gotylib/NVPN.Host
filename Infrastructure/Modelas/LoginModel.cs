namespace Infrastructure.Dto;

public class LoginModel
{
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Пароля пользователя.
    /// </summary>
    public required string Password { get; set; }
}