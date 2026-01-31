namespace DAL.Entities;

public class Server
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// IP или домен
    /// </summary>
    public string Host { get; set; }


    /// <summary>
    /// Страна расположения
    /// </summary>
    public string Country { get; set; }


    // Админские данные для управления

    /// <summary>
    /// Логин от админки
    /// </summary>
    public string AdminLogin { get; set; }

    /// <summary>
    /// Пароль от админки
    /// </summary>
    public string AdminPassword { get; set; }

    // Навигационные свойства

    /// <summary>
    /// Vless конфигурации для подключения пользователей к этому серверу
    /// </summary>
    public List<VlessLink> VlessLinks { get; set; } = new List<VlessLink>();

    /// <summary>
    /// Пользователи на этом сервере
    /// </summary>
    public List<User> Users { get; set; } = new List<User>();
}