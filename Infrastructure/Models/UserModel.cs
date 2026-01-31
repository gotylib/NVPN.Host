namespace Infrastructure.Dto;

/// <summary>
/// Модель пользователя.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Id Пользователя.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя полностью. (ФИО).
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Пароля пользователя.
    /// </summary>
    public required string Password { get; set; }
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Vless ссылки на vpn.
    /// </summary>
    public string? VpnLinks { get; set; }
    
    /// <summary>
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    public string? ServerName { get; set; }
}