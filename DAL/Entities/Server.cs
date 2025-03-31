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
    /// Ссылка на сервер к которому привязан пользоваетль.
    /// </summary>
    [MaxLength(100)]
    [Comment("Ссылка на сервер к которому привязан пользоваетль.")]
    public string ServerName { get; set; }
}