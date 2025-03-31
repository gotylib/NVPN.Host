using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

public class User
{
    /// <summary>
    /// Id Пользователя.
    /// </summary>
    [Key]
    [Comment("Id Пользователя.")]
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя полностью. (ФИО).
    /// </summary>
    [MaxLength(100)]
    [Comment("Имя пользователя полностью. (ФИО).")]
    public string Username { get; set; }
    
    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    [Comment("Хэш пароля пользователя.")]
    public string? HashPassword { get; set; }
    
    /// <summary>
    /// Vless ссылки на vpn.
    /// </summary>
    [MaxLength(100)]
    [Comment("Vless ссылки на vpn.")]
    public string VpnLinks { get; set; }
    
    /// <summary>
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    [MaxLength(100)]
    [Comment("Ссылка на сервер к которому привязан пользоваетль.")]
    public string ServerName { get; set; }
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    [MaxLength(100)]
    [Comment("Email пользователя.")]
    public string? Email { get; set; }
    
}