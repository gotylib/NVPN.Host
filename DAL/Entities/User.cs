namespace DAL.Entities;

/// <summary>
/// Пользователь.
/// </summary>

public class User
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string HashPassword { get; set; }

    /// <summary>
    /// Случайная соль
    /// </summary>
    public string Salt { get; set; }

    // Навигационные свойства
    /// <summary>
    /// Vless конфигурации для подключения пользователей
    /// </summary>
    public List<VlessLink> VlessLinks { get; set; } = new List<VlessLink>();

    /// <summary>
    /// Сервера, доступные пользователю
    /// </summary>
    public List<Server> Servers { get; set; } = new List<Server> ();
}