using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class VpnDbContext(DbContextOptions<VpnDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Server> Servers { get; set; }
    
    public DbSet<VlessProfile>  VlessProfiles { get; set; }
}