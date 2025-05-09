using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

/// <summary>
/// Пользователь.
/// </summary>
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
    /// Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.
    /// </summary>
    [MaxLength(32)]
    [Comment("Случайная строка, которая добавляется к паролю для того, чтьо бы одинаковые пароли были с разным хэшем.")]
    public string Salt { get; set; }
    
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
    public required string Email { get; set; }
    
    /// <summary>
    /// Флаг отвечающий за проверку Email.
    /// </summary>
    public required bool IsEmailConfirmed { get; set; }
}