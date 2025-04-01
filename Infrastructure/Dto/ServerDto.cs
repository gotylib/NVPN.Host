namespace Infrastructure.Dto;

public class ServerDto
{
    /// <summary>
    /// Id Сервера.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Админский логин сервера
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Админский пароль сервера
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    public string ServerName { get; set; }
}