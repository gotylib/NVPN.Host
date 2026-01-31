using DAL.Entities;

public class UserServer
{

    // промежуточная сущность для связи многие ко многим

    public int UserId { get; set; }
    public User User { get; set; }

    public int ServerId { get; set; }
    public Server Server { get; set; }

}
