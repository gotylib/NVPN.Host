namespace DAL.Entities;

/// <summary>
/// Vless ссылка пользователя.
/// </summary>
public class VlessLink
{
    /// <summary>
    /// Id ссылки
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// Vless ссылка
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Дата создания ссылки
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата окончания действия ссылки
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Активна ли ссылка.
    /// </summary>
    public bool IsActive { get; set; }

    
    public int UserId { get; set; }

    public User User { get; set; }

    
    public int ServerId { get; set; }

    public Server Server { get; set; }

}