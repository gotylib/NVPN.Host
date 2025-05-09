using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

public class Server
{
    /// <summary>
    /// Id Сервера.
    /// </summary>
    [Key]
    [Comment("Id Сервера.")]
    public int Id { get; set; }
    
    /// <summary>
    /// Админский логин сервера
    /// </summary>
    [MaxLength(50)]
    [Comment("Админский логин сервера")]
    public string Login { get; set; }
    
    /// <summary>
    /// Хэшированный админский пароль сервера
    /// </summary>
    [MaxLength(50)]
    [Comment("Хэшированный админский пароль сервера")]
    public string HashPassword { get; set; }
    
    /// <summary>
    /// Случайная строка, которая добавляется к паролю для того, что бы одинаковые пароли были с разным хэшем.
    /// </summary>
    [MaxLength(32)]
    [Comment("Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.")]
    public string Salt { get; set; }
    
    /// <summary>
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    [MaxLength(100)]
    [Comment("Ссылка на сервер к которому привязан пользоваетль.")]
    public string ServerName { get; set; }
}