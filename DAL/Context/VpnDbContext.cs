using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class VpnDbContext(DbContextOptions<VpnDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<VlessLink> VlessLinks { get; set; }

    public DbSet<UserServer> UserServers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ServerConfiguration());
        modelBuilder.ApplyConfiguration(new VlessLinkConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}