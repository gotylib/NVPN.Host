namespace Infrastructure.Dto;

/// <summary>
/// Модель пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Id Пользователя.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя полностью. (ФИО).
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Пароля пользователя.
    /// </summary>
    public string? Password { get; set; }
    
    /// <summary>
    /// Vless ссылки на vpn.
    /// </summary>
    public string VpnLinks { get; set; }
    
    /// <summary>
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    public string ServerName { get; set; }
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string? Email { get; set; }
}